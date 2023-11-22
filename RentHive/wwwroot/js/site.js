/*dropdown*/
document.addEventListener("DOMContentLoaded", function () {
    var dropdown = document.querySelector('.dropdown');

    dropdown.addEventListener('click', function () {
        dropdown.classList.toggle('active');
    });
});

/* Password unhide*/
document.addEventListener("DOMContentLoaded", function () {
    const passwordInput = document.querySelector('.password-text');
    const showIcon = document.getElementById('showIcon');
    const hideIcon = document.getElementById('hideIcon');

    showIcon.addEventListener("click", () => {
        passwordInput.type = 'text';
        showIcon.style.display = 'none';
        hideIcon.style.display = 'inline';
    });

    hideIcon.addEventListener("click", () => {
        passwordInput.type = 'password';
        showIcon.style.display = 'inline';
        hideIcon.style.display = 'none';
    });
});

/* Update Profile user*/
document.addEventListener("DOMContentLoaded", () => {
    const editButton = document.querySelector(".editButton");
    const saveButton = document.querySelector(".saveButton");

    // Select the input fields by their IDs
    const userDisplayName = document.querySelector("#userDisplayName");
    const userFirstName = document.querySelector("#userFirstName");
    const userMiddleName = document.querySelector("#userMiddleName");
    const userLastName = document.querySelector("#userLastName");
    const userPhoneNum = document.querySelector("#userPhoneNum");
    const userBirthDate = document.querySelector("#userBirthDate");
    const userAddress = document.querySelector("#userAddress");
    const userEnail = document.querySelector("#userEmail");
    const userPassword = document.querySelector("#userPassword");

    editButton.addEventListener("click", () => {
        // Remove the readonly attribute
        userDisplayName.removeAttribute("readonly");
        userFirstName.removeAttribute("readonly");
        userMiddleName.removeAttribute("readonly");
        userLastName.removeAttribute("readonly");
        userPhoneNum.removeAttribute("readonly");
        userBirthDate.removeAttribute("readonly");
        userAddress.removeAttribute("readonly");
        userEnail.removeAttribute("readonly");
        userPassword.removeAttribute("readonly");

        // Toggle button visibility
        editButton.style.display = "none";
        saveButton.style.display = "inline-block";
    });

    saveButton.addEventListener("click", () => {
        // Add the readonly attribute back
        userDisplayName.setAttribute("readonly", "readonly");
        userFirstName.setAttribute("readonly", "readonly");
        userMiddleName.setAttribute("readonly", "readonly");
        userLastName.setAttribute("readonly", "readonly");
        userPhoneNum.setAttribute("readonly", "readonly");
        userBirthDate.setAttribute("readonly", "readonly");
        userAddress.setAttribute("readonly", "readonly");
        userEmail.setAttribute("readonly", "readonly");
        userPassword.setAttribute("readonly", "readonly");

        // Toggle button visibility
        editButton.style.display = "inline-block";
        saveButton.style.display = "none";
    });
});

/*DELETE LATER*/
/*document.addEventListener("DOMContentLoaded", function () {
    const startDateInput = document.getElementById("startDate");
    const endDateInput = document.getElementById("endDate");
    const resultInput = document.getElementById("result");
    const timeInput = document.getElementById("setTimeBan");

    startDateInput.addEventListener("input", calculateRemainingDays);
    endDateInput.addEventListener("input", calculateRemainingDays);

    function calculateRemainingDays() {
        const startDate = new Date(startDateInput.value + "T00:00:00"); // Set time to midnight
        const endDate = new Date(endDateInput.value + "T00:00:00");     // Set time to midnight
        const currentDate = new Date();

        if (startDate > endDate) {
            resultInput.value = "End date cannot be before the start date.";
        } else {
            const timeDifference = endDate - currentDate;
            const remainingDays = Math.ceil(timeDifference / (1000 * 60 * 60 * 24));

            if (remainingDays < 0) {
                resultInput.value = "The end date is in the past.";
            } else {
                resultInput.value = remainingDays;
                timeInput.value = remainingDays;
            }
        }
    }
});*/


