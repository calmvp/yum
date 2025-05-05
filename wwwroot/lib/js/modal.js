window.openModal = function (modalId) {
    bootstrap.Modal.getOrCreateInstance(document.getElementById(modalId)).show();
}

window.closeModal = function (modalId) {
    bootstrap.Modal.getOrCreateInstance(document.getElementById(modalId)).hide();
}