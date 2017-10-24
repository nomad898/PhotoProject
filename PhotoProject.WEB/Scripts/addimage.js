var loadFile = function (event) {
    var output = document.getElementById('avatarImg');
    output.src = URL.createObjectURL(event.target.files[0]);
};

