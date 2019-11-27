$( document ).ready(function() {

    $("#fileUploadInput").change(function(){

        var files = $("#fileUploadInput").prop("files");
        var formData = new FormData();
        for(let i = 0; i != files.length; i++){
            console.log("i: " + i + " File Name: " + files[i].name);
            formData.append("files", files[i]);
        }

         $.ajax({
            type: "POST",
             url: "https://localhost:44336/api/upload",
             data: formData,
             contentType: false,
             processData: false,
             success: function(data){
                 alert("Hit Upload");
             },
             error: function(data){
                alert("Error, did not hit.");
             }
        });
    });
});