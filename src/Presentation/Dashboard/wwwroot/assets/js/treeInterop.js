function initializeTree(config) {
    var my_chart = new Treant(config);

    var treeNodes = document.querySelectorAll('.node');
    treeNodes.forEach(function (node) {
        node.addEventListener('click', function (event) {
            var nodeElement = event.target.closest('.node');
            if (!nodeElement) return;

            var nodeNameElement = nodeElement.querySelector('.node-name');
            if (!nodeNameElement) return;

            var nodeName = nodeNameElement.innerText;

            if (nodeName.toLowerCase() === "moderator") {
                $('#Moderator').modal('show');
            } else if (nodeName.toLowerCase() === "empty") {
                $('#emptyModalCenter').modal('show');
            } else {
                $('#exampleModalCenter').modal('show');
            }
        });
    });
}

function navigatePage() {
    var select = document.getElementById("pageSelect");
    var selectedValue = select.value;
    if (selectedValue) {
        window.location.href = selectedValue;
    }
}

function navigate2Page() {
    var select = document.getElementById("page2Select");
    var selectedValue = select.value;
    if (selectedValue) {
        window.location.href = selectedValue;
    }
}