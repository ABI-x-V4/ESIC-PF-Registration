

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

    bindPfFilePreview("aadhaarFile", "aadhaarPreview");
    bindPfFilePreview("panFile", "panPreview");
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

function bindPfFilePreview(inputId, previewId) {

    const input = document.getElementById(inputId);
    const style = document.createElement("style");

    style.innerHTML = `
                        .thumbb .img-thumbnail-clickable {
                            cursor: pointer;
                            transition: opacity 0.2s;
                        }

                            .thumbb .img-thumbnail-clickable:hover {
                                opacity: 0.8;
                            }

                        .thumbb img {
                            height: 100px;
                            width: 100px;
                            object-fit: cover;
                        }

                        .b-red {
                            background: red;
                            background-color: red;
                            border: 1px solid #fff;
                            --bs-btn-close-bg: none;
                            opacity: 1;
                            color: #fff;
                            line-height: 15px;
                        }

                            .b-red:hover {
                                color: #fff !important;
                                opacity: 1 !important;
                            }

                        .pdf-thumbb img {
                            cursor: pointer;
                            transition: opacity 0.2s;
                            height: 100px;
                            width: 100px;
                            object-fit: cover;
                            padding: .25rem;
                            background-color: #fff;
                            border: 1px solid rgb(222 226 230);
                            border-radius: 0.375rem;
                            max-width: 100%;
                        }
                            `;
    document.head.appendChild(style);
    input.addEventListener("change", function () {

        const preview = document.getElementById(previewId);

        preview.innerHTML = "";

        if (this.files.length === 0)
            return;

        const file = this.files[0];

        const extension = file.name.split('.').pop().toLowerCase();

        const fileUrl = URL.createObjectURL(file);

        const modalId = `${inputId}_FileModal`;

        // IMAGE
        if (["jpg", "jpeg"].includes(extension)) {

            preview.innerHTML = `
            
                <div class="thumbb text-center">                   
                    <div class="mt-2">
                        <a href="javascript:void(0)" class="el-act act-view" data-bs-toggle="modal" data-bs-target="#${modalId}" title="View" style="width:80px !important;">
                            <i class="ri-eye-line me-1"></i> View
                        </a>
                    </div>
                </div>

                <!-- Modal -->
                <div class="modal fade" id="${modalId}" tabindex="-1" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered modal-lg">
                        <div class="modal-content border-0">
                            <div class="modal-body p-0">
                                <button type="button" class="btn-close b-red position-absolute top-0 end-0 m-3" data-bs-dismiss="modal">
                                    <i class="ri-close-line"></i>
                                </button>
                                <img src="${fileUrl}" class="img-fluid rounded w-100" />
                            </div>
                        </div>
                    </div>
                </div>
            `;
        }

        // PDF
        else if (extension === "pdf") {

            preview.innerHTML = `
            
                <div class="pdf-thumbb text-center">
                    <div class="mt-2">
                        <a href="javascript:void(0)" class="el-act act-view" data-bs-toggle="modal" data-bs-target="#${modalId}" title="View"  style="width:80px !important;">
                            <i class="ri-eye-line me-1"></i> View
                        </a>
                    </div>
                </div>

                <!-- Modal -->
                <div class="modal fade" id="${modalId}" tabindex="-1" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered modal-xl">
                        <div class="modal-content">     
                            <div class="modal-body p-0">
                                  <button type="button" class="btn-close b-red position-absolute top-0 end-0 m-3" data-bs-dismiss="modal">
                                    <i class="ri-close-line"></i>
                                </button>
                                <iframe src="${fileUrl}"
                                        style="width:100%;height:80vh;border:0;">
                                </iframe>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        }

        else {

            preview.innerHTML = `
                <div class="text-danger">
                    Unsupported file type.
                </div>
            `;
        }

    });
}

