// Modal helper functions for Bootstrap 5 modals

window.showModal = function (modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
    } else {
        console.error(`Modal with id '${modalId}' not found`);
    }
};

window.hideModal = function (modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modal = bootstrap.Modal.getInstance(modalElement);
        if (modal) {
            modal.hide();
        } else {
            // If no instance exists, create one and hide it
            const newModal = new bootstrap.Modal(modalElement);
            newModal.hide();
        }
    } else {
        console.error(`Modal with id '${modalId}' not found`);
    }
};

// Alternative: dispose modal completely
window.disposeModal = function (modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modal = bootstrap.Modal.getInstance(modalElement);
        if (modal) {
            modal.dispose();
        }
    }
};

// Check if modal is currently shown
window.isModalShown = function (modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        return modalElement.classList.contains('show');
    }
    return false;
};