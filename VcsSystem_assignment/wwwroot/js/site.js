<script>
    function validateDOB() {
    const dobInput = document.getElementById("dob").value;
    const dob = new Date(dobInput);
    const today = new Date();

    if (dob >= today) {
        alert("Date of Birth must be less than the current date.");
    return false;
    }
    return true;
}

    function validateEmail() {
    const emailInput = document.getElementById("email").value;
    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (!emailPattern.test(emailInput)) {
        alert("Please enter a valid email address.");
    return false;
    }
    return true;
}

    function validateFileUpload() {
    const fileInput = document.getElementById("fileUpload");
    const filePath = fileInput.value;
    const allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;

    if (!allowedExtensions.exec(filePath)) {
        alert("Please upload a file with a valid image extension (.jpg, .jpeg, .png).");
    fileInput.value = ''; // Clear the input
    return false;
    }
    return true;
}

    function validateForm() {
    return validateDOB() && validateEmail() && validateFileUpload();
}
</script>
