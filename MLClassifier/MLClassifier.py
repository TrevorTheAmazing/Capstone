
import datetime
import math
import os
import librosa
from sklearn.model_selection import train_test_split
import keras
from keras.models import Sequential
from keras.layers import Dense, Dropout, Flatten, Conv2D, MaxPooling2D
from keras.utils import to_categorical
import numpy as np
from tqdm import tqdm
import tensorflow
import tensorboard

#PREDICTION_DIRECTORY = sys.argv[1]

#0. start tb
#tensorboard --logdir logs/fit

#I. set the data path
DATA_PATH = "C:\\Users\\Trevor\\Dropbox\\dcc\\capstone\\Capstone\\MLClassifier\\mLprojData\\Data\\"
PREDICTIONS_PATH = "C:\\Users\\Trevor\\Dropbox\\dcc\\capstone\\Capstone\\MLClassifier\\mLprojData\\Predict\\"
RESULTS_PATH = "C:\\Users\\Trevor\\Dropbox\\dcc\\capstone\\Capstone\\MLClassifier\\mLprojData\\Results\\"
print('DataPath is set to '+DATA_PATH)
########################
#######Get_Labels#######
########################
# Input: Folder Path                                                  #
# Output: Tuple (Label, Indices of the labels, one-hot encoded labels)#
def get_labels(path=DATA_PATH):
    labels = os.listdir(path)
    label_indices = np.arange(0, len(labels))
    return labels, label_indices, to_categorical(label_indices)


#######wav2mfcc#######
# convert .wav to mfcc
def wav2mfcc(file_path, max_len=11):
    wave, sr = librosa.load(file_path, mono=True, sr=22050)#sample rate
    wave = wave[::3]
    mfcc = librosa.feature.mfcc(wave, sr=22050)#sample rate

    # If maximum length exceeds mfcc lengths then pad
    if (max_len > mfcc.shape[1]):
        pad_width = max_len - mfcc.shape[1]
        mfcc = np.pad(mfcc, pad_width=((0, 0), (0, pad_width)), mode='constant')

    # Else cutoff the remaining parts
    else:
        mfcc = mfcc[:, :max_len]

    return mfcc

################################
#######save_data_to_array#######
################################
def save_data_to_array(path=DATA_PATH, max_len=11):
    labels, _, _ = get_labels(path)

    for label in labels:
        # Init mfcc vectors
        mfcc_vectors = []

        wavfiles = [path + label + '\\' + wavfile for wavfile in os.listdir(path + '\\' + label)]
        for wavfile in tqdm(wavfiles, "Saving vectors of label - '{}'".format(label)):
            mfcc = wav2mfcc(wavfile, max_len=max_len)
            mfcc_vectors.append(mfcc)
        np.save(label + '.npy', mfcc_vectors)

############################
#######get_train_test#######
############################
def get_train_test(split_ratio=0.9, random_state=42):
    # Get labels
    labels, indices, _ = get_labels(DATA_PATH)

    # Getting first arrays
    X = np.load(labels[0] + '.npy')
    y = np.zeros(X.shape[0])

    # Append all of the dataset into one single array, same goes for y
    for i, label in enumerate(labels[1:]):
        x = np.load(label + '.npy')
        X = np.vstack((X, x))
        y = np.append(y, np.full(x.shape[0], fill_value=(i + 1)))

    assert X.shape[0] == len(y)

    return train_test_split(X, y, test_size=(1 - split_ratio), random_state=random_state, shuffle=True)

##########################
#######load_dataset#######
##########################
def load_dataset(path=DATA_PATH):
    data = prepare_dataset(path)
    
    dataset = []

    for key in data:
        for mfcc in data[key]['mfcc']:
            dataset.append((key, mfcc))

    return dataset[:100]

#II. Second dimension of the feature is dim2
feature_dim_2 = 11

#III. Save data to array file first
save_data_to_array(max_len=feature_dim_2)

# # Loading train set and test set
X_train, X_test, y_train, y_test = get_train_test()

# # Feature dimension
feature_dim_1 = 20
channel = 1
epochs = 3
batch_size = 1
verbose = 1
num_classes = 3

# Reshaping to perform 2D convolution
X_train = X_train.reshape(X_train.shape[0], feature_dim_1, feature_dim_2, channel)
X_test = X_test.reshape(X_test.shape[0], feature_dim_1, feature_dim_2, channel)

y_train_hot = to_categorical(y_train)
y_test_hot = to_categorical(y_test)


def get_model():
    print("get_model")
    model = Sequential()
    model.add(Conv2D(32, kernel_size=(3, 3), activation='relu', input_shape=(feature_dim_1, feature_dim_2, channel)))
    model.add(Conv2D(48, kernel_size=(3, 3), activation='relu'))
    #model.add(Conv2D(120, kernel_size=(2, 2), activation='relu'))
    model.add(MaxPooling2D(pool_size=(3, 3)))
    model.add(Dropout(0.25))
    model.add(Flatten())
    #model.add(Dense(128, activation='relu'))
    model.add(Dropout(0.25))
    model.add(Dense(64, activation='relu'))
    model.add(Dropout(0.4))
    model.add(Dense(num_classes, activation='softmax'))
    model.compile(loss=keras.losses.categorical_crossentropy,
                  optimizer = keras.optimizers.Adam(),
                  metrics=['accuracy'])
    return model

# Predicts one sample
def predict(filepath, model):
    print('predicting '+filepath)
    sample = wav2mfcc(filepath)
    sample_reshaped = sample.reshape(1, feature_dim_1, feature_dim_2, channel)
    return get_labels()[0][np.argmax(model.predict(sample_reshaped))]


def prepare_dataset(path=DATA_PATH):
    labels, _, _ = get_labels(path)
    data = {}
    for label in labels:
        print('preparing ' + label + ' dataset')
        data[label] = {}
        data[label]['path'] = [path  + label + '/' + wavfile for wavfile in os.listdir(path + '/' + label)]

        vectors = []

        for wavfile in data[label]['path']:
            wave, sr = librosa.load(wavfile, mono=True, sr=22050)#sample rate
            # Downsampling
            wave = wave[::3]
            mfcc = librosa.feature.mfcc(wave, sr=22050)#sample rate
            vectors.append(mfcc)

        data[label]['mfcc'] = vectors

    return data

#prepare the dataset
prepare_dataset(DATA_PATH)

#tensorboard logs
#file_writer = tensorflow.summary.FileWriter('C:\\Users\\Trevor\\Dropbox\\dcc\\capstone\\capstone\\Capstone\\mLprojData\\Logs\\', sess.graph)
current_time = datetime.datetime.now().strftime("%Y%m%d-%H%M%S")
logdir="logs\\fit\\" + current_time #tlc
tensorboard_callback = keras.callbacks.TensorBoard(log_dir=logdir)

model = get_model()
model.fit(X_train, y_train_hot, batch_size=batch_size, epochs=epochs, verbose=verbose, validation_data=(X_test, y_test_hot), callbacks=[tensorboard_callback])


#predict
#print(predict('C:\\Users\\Trevor\\Dropbox\\dcc\\capstone\\capstone\\Capstone\\mLprojData\\Predict\\an2.wav', model=model))
#print(predict('C:\\Users\\Trevor\\Dropbox\\dcc\\capstone\\capstone\\Capstone\\mLprojData\\Predict\\STE-000.WAV', model=model))
#print(predict('C:\\Users\\Trevor\\Dropbox\\dcc\\capstone\\capstone\\Capstone\\mLprojData\\Predict\\STE-002.WAV', model=model))
#print(predict('C:\\Users\\Trevor\\Dropbox\\dcc\\capstone\\capstone\\Capstone\\mLprojData\\Predict\\STE-003.WAV', model=model))

results = list()
uploadedFiles=os.listdir(PREDICTIONS_PATH)

results_file = open(os.path.join(RESULTS_PATH, current_time +'.txt'), "w")

for fileUpload in uploadedFiles:
    tempResults = predict(PREDICTIONS_PATH+fileUpload, model=model)
    tempResults += " - "+PREDICTIONS_PATH+fileUpload + '\n'
    print(tempResults)
    results_file.write(tempResults)
    #results.append(tempResults)
results_file.close()
exit()
    



