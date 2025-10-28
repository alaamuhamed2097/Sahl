// Updated JavaScript with better error handling and debugging
window.initializeSmartWizard = function (currentState) {
    try {
        console.log('Initializing SmartWizard with state:', currentState);

        $('#smartwizard').smartWizard({
            theme: 'arrows',
            transitionEffect: 'fade',
            showStepURLhash: false,
            rtl: false,
            toolbarSettings: {
                toolbarPosition: 'none'
            },
            // Set initial step based on current state
            selected: getStepFromState(currentState),
            // Disable navigation clicks to prevent manual step changes
            enableURLhash: false,
            enableAllSteps: false,
            // Add debugging
            onShowStep: function (e, anchorObject, stepIndex, stepDirection) {
                console.log('SmartWizard showing step:', stepIndex, 'Direction:', stepDirection);
            }
        });

        // Disable manual step navigation
        $("#smartwizard ul.step-anchor li a").on("click", function (e) {
            e.preventDefault();
            return false;
        });

        console.log('SmartWizard initialized successfully');
    } catch (error) {
        console.error('Error initializing SmartWizard:', error);
    }
};

// Helper function to map order status to wizard step
function getStepFromState(currentState) {
    const stepMap = {
        1: 0, // Pending -> Step 1
        2: 0, // Accepted -> Step 1 (but ready to move to step 2)
        3: 0, // Rejected -> Step 1
        4: 0, // InProgress -> Step 1
        5: 1, // Shipping -> Step 2
        6: 2  // Delivered -> Step 3
    };

    const step = stepMap[parseInt(currentState)] || 0;
    console.log('Mapping state', currentState, 'to step', step);
    return step;
}

// Function to programmatically go to a specific step
window.goToStep = function (stepIndex) {
    try {
        console.log('Going to step:', stepIndex);
        $('#smartwizard').smartWizard("goToStep", stepIndex);
    } catch (error) {
        console.error('Error going to step:', error);
    }
};

// Function to move to next step - call this from C#
window.moveToNextStep = function () {
    try {
        console.log('Moving to next step');
        $('#smartwizard').smartWizard("next");
    } catch (error) {
        console.error('Error moving to next step:', error);
    }
};

// Function to update wizard based on new state
window.updateWizardState = function (newState) {
    try {
        console.log('Updating wizard state to:', newState);
        const stepIndex = getStepFromState(newState);
        $('#smartwizard').smartWizard("goToStep", stepIndex);
    } catch (error) {
        console.error('Error updating wizard state:', error);
    }
};

// Function to check if SmartWizard is initialized
window.isSmartWizardReady = function () {
    return $('#smartwizard').length > 0 && $('#smartwizard').hasClass('sw-main');
};