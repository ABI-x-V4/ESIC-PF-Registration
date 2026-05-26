

let lastValidFileName = "";

document.getElementById("aadhaarFile").addEventListener("change", function () {

    if (this.files.length === 0) return;

    const file = this.files[0];
    const fileName = file.name;
    const extension = fileName.split('.').pop().toLowerCase();

    if (extension !== "pdf") {
        alert("Only PDF files are allowed.");

        this.value = "";
        return;
    }
    lastValidFileName = fileName;
    document.getElementById("aadhaarText").textContent = fileName;
});


let lastValidFileName2 = "";

document.getElementById("panFile").addEventListener("change", function () {

    if (this.files.length === 0) return;

    const file = this.files[0];
    const fileName = file.name;
    const allowedExtensions = ["pdf", "jpg", "jpeg"];

    const extension = fileName.split('.').pop().toLowerCase();

    if (!allowedExtensions.includes(extension)) {
        alert("Only PDF and JPG files are allowed.");

        this.value = "";

        return;
    }

    lastValidFileName2 = fileName;
    document.getElementById("panText").textContent = fileName;
});


document.querySelector('[name="Uan"]').addEventListener("paste", function (e) {
    let pasted = (e.clipboardData || window.clipboardData).getData('text');
    if (!/^\d+$/.test(pasted)) {
        e.preventDefault();
        alert("Only numeric values allowed.");
    }
});

