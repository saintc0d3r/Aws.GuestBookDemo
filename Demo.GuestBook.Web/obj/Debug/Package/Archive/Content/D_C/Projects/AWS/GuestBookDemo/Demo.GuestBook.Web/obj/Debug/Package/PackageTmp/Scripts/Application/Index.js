
$(document).ready(function () {
    var index = new Index();
    
    $('#imageFileUpload').bind('change', index.onFileSelected);
});

// Class Index
function Index() {
    // Method onFileSelected
    this.onFileSelected = function(evt) {
        var uploadedFile = evt.target.files[0];

        // Read uploaded file using HTML5 API ( FileReader )
        var fileReader = new FileReader();
        fileReader.onload = function (fileReaderEvent) {
            var imageElement = ['<img src="', fileReaderEvent.target.result, '" title="', uploadedFile.name, '" width=', 320, ' />'].join('');
            $('#image-box').prop('innerHTML', imageElement);
        };

        fileReader.readAsDataURL(uploadedFile);
    };
}
