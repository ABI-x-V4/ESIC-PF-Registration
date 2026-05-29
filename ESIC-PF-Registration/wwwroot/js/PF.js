

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



document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("employeePFForm");
    const btnPreview = document.getElementById("btnPreview");
    const btnEdit = document.getElementById("btnEdit");
    const btnFinalSubmit = document.getElementById("btnFinalSubmit");
    const previewBanner = document.getElementById("previewBanner");

    function getFormControls() {
        return form.querySelectorAll("input, select, textarea, button");
    }

    function setPreviewMode(enablePreview) {
        const controls = getFormControls();

        controls.forEach(control => {
            const type = (control.type || "").toLowerCase();
            const id = control.id || "";

            // not disabling hidden fields and action buttons
            if (
                type === "hidden" ||
                id === "btnPreview" ||
                id === "btnEdit" ||
                id === "btnFinalSubmit"
            ) {
                return;
            }

            if (enablePreview) {
                control.setAttribute("disabled", "disabled");
            } else {
                control.removeAttribute("disabled");
            }
        });

        if (enablePreview) {
            form.classList.add("preview-mode");
            previewBanner.style.display = "flex";
            btnPreview.style.display = "none";
            btnEdit.style.display = "inline-block";
            btnFinalSubmit.style.display = "inline-block";
        } else {
            form.classList.remove("preview-mode");
            previewBanner.style.display = "none";
            btnPreview.style.display = "inline-block";
            btnEdit.style.display = "none";
            btnFinalSubmit.style.display = "none";
        }
    }

    btnPreview.addEventListener("click", function () {
        
        if (window.jQuery && $(form).valid) {
            if (!$(form).valid()) {
                return;
            }
        } else {
            
            if (!form.checkValidity()) {
                form.reportValidity();
                return;
            }
        }

        setPreviewMode(true);
        window.scrollTo({ top: 0, behavior: "smooth" });
    });

    btnEdit.addEventListener("click", function () {
        setPreviewMode(false);
    });

    form.addEventListener("submit", function () {
        //  before final submit, enable everything again.
        const controls = getFormControls();
        controls.forEach(control => {
            const type = (control.type || "").toLowerCase();
            if (type !== "button") {
                control.removeAttribute("disabled");
            }
        });
    });

    // File name display
    const aadhaarFile = document.getElementById("aadhaarFile");
    const aadhaarText = document.getElementById("aadhaarText");
    if (aadhaarFile && aadhaarText) {
        aadhaarFile.addEventListener("change", function () {
            aadhaarText.textContent = this.files.length > 0 ? this.files[0].name : "No file selected";
        });
    }

    const panFile = document.getElementById("panFile");
    const panText = document.getElementById("panText");
    if (panFile && panText) {
        panFile.addEventListener("change", function () {
            panText.textContent = this.files.length > 0 ? this.files[0].name : "No file selected";
        });
    }
});

