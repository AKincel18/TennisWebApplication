$(document).ready(function () {

    $("#upload").change(function () {

        var File = this.files;

        if (File && File[0]) {
            ReadImage(File[0]);
        }

    })


})


var ReadImage = function (file) {

    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {

        image.src = _file.target.result;
        image.onload = function () {

            var height = this.height;
            var width = this.width;
            var type = file.type;
            var size = ~~(file.size / 1024) + "KB";

            $("#targetImg").attr('src', _file.target.result);
            $("#description").text("Size:" + size + ", " + height + "x" + width + ", " + type + "");
            $("#imgPreview").show();

        }

    }

}

var ClearPreview = function () {
    $("#upload").val('');
    $("#description").text('');
    $("#imgPreview").hide();

}