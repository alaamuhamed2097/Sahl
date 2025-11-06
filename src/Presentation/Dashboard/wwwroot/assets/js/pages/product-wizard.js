// Product Wizard Management
'use strict';

// Initialize Smart Wizard for Products
window.initializeSmartWizard = function (initialStep = 0) {
    setTimeout(function () {
        $('#smartwizard').smartWizard({
     theme: 'arrows',
      transitionEffect: 'fade',
            autoAdjustHeight: true,
      useURLhash: false,
 showStepURLhash: false,
     toolbarSettings: {
        toolbarPosition: 'none' // Hide default toolbar, we'll use custom buttons
            },
            anchorSettings: {
  anchorClickable: false, // Disable clicking on step numbers
     enableAllAnchors: false, // Disable all anchors initially
                markDoneStep: true,
         markAllPreviousStepsAsDone: true,
           removeDoneStepOnNavigateBack: false,
    enableAnchorOnDoneStep: true
    },
 keyboardSettings: {
        keyNavigation: false // Disable keyboard navigation
            }
      });

    // Go to initial step if specified
        if (initialStep > 0) {
   $('#smartwizard').smartWizard("goToStep", initialStep);
        }

        // Prevent default anchor click behavior
        $('#smartwizard .nav-link').on('click', function(e) {
       e.preventDefault();
      return false;
     });
    }, 700);
};

// Move to next step
window.moveToNextStep = function () {
    $('#smartwizard').smartWizard("next");
    return true;
};

// Move to previous step
window.moveToPreviousStep = function () {
    $('#smartwizard').smartWizard("prev");
    return true;
};

// Go to specific step
window.goToStep = function (stepIndex) {
    $('#smartwizard').smartWizard("goToStep", stepIndex);
};

// Update wizard state (for editing existing products)
window.updateWizardState = function (stepIndex) {
 $('#smartwizard').smartWizard("goToStep", stepIndex);
};

// Get current step index
window.getCurrentStep = function () {
    var currentIndex = $('#smartwizard').smartWizard("getStepIndex");
    return currentIndex;
};

// Reset wizard to first step
window.resetWizard = function () {
    $('#smartwizard').smartWizard("reset");
};
