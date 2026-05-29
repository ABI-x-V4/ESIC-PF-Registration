$(function () {

    $(document).on("change", ".stateDropdown", function () {   
        var stateId = $(this).val();
        var targetDistrictId = $(this).data("target");
        var districtDropdown = $("#" + targetDistrictId);

        districtDropdown.empty()
            .append('<option value="">-- Select District --</option>');

        if (!stateId) return;

        $.get('/Employee/GetDistrictsByState', { stateId: stateId }, function (data) {
            console.log(data);
            $.each(data, function (i, item) {
                console.log(item); 
                districtDropdown.append(
                    '<option value="' + item.id + '">' + item.districtName + '</option>'
                );
            });
        });

    });

});

//Address
$(function () {

    const checkbox = document.getElementById("sameAddressCheck");

    const present = document.getElementById("presentAddress");
    const presentStateID = document.getElementById("presentstateDropdown");
    const presentDistrictID = document.getElementById("presentdistrictDropdown");
    const presentPinCode = document.getElementById("presentPinCode");
    const presentMobile = document.getElementById("presentMobile");
    const presentEmail = document.getElementById("presentEmail");

    const permanent = document.getElementById("permanentAddress");
    const permanentStateID = document.getElementById("permanentstateDropdown");
    const permanentDistrictID = document.getElementById("permanentdistrictDropdown");
    const permanentPinCode = document.getElementById("permanentPinCode");
    const permanentMobile = document.getElementById("permanentMobile");
    const permanentEmail = document.getElementById("permanentEmail");

    function copyAddress() {
        permanent.value = present.value;
        permanentPinCode.value = presentPinCode.value;
        permanentMobile.value = presentMobile.value;
        permanentEmail.value = presentEmail.value;

        permanentStateID.value = presentStateID.value;

        $(permanentStateID).trigger('change');

        setTimeout(function () {
            permanentDistrictID.value = presentDistrictID.value;
        }, 300); 
    }

    checkbox.addEventListener("change", function () {

        if (this.checked) {
            copyAddress();

            permanent.readOnly = true;
            permanentPinCode.readOnly = true;
            permanentMobile.readOnly = true;
            permanentEmail.readOnly = true;

            //permanentStateID.disabled = true;
            //permanentDistrictID.disabled = true;

            permanentStateID.style.pointerEvents = "none";
            permanentDistrictID.style.pointerEvents = "none";
            permanentStateID.classList.add("bg-light");
            permanentDistrictID.classList.add("bg-light");


        } else {
            permanent.readOnly = false;
            permanentPinCode.readOnly = false;
            permanentMobile.readOnly = false;
            permanentEmail.readOnly = false;

            //permanentStateID.disabled = false;
            //permanentDistrictID.disabled = false;

            permanentStateID.style.pointerEvents = "auto";
            permanentDistrictID.style.pointerEvents = "auto";
            permanentStateID.classList.remove("bg-light");
            permanentDistrictID.classList.remove("bg-light");


            permanent.value = "";
            permanentStateID.value = "";
            permanentDistrictID.value = "";
            permanentPinCode.value = "";
            permanentMobile.value = "";
            permanentEmail.value = "";
        }
    });

    present.addEventListener("input", function () {
        if (checkbox.checked) copyAddress();
    });

    presentPinCode.addEventListener("input", function () {
        if (checkbox.checked) permanentPinCode.value = presentPinCode.value;
    });

    presentMobile.addEventListener("input", function () {
        if (checkbox.checked) permanentMobile.value = presentMobile.value;
    });

    presentEmail.addEventListener("input", function () {
        if (checkbox.checked) permanentEmail.value = presentEmail.value;
    });

    presentStateID.addEventListener("change", function () {
        if (checkbox.checked) copyAddress();
    });

    presentDistrictID.addEventListener("change", function () {
        if (checkbox.checked) {
            permanentDistrictID.value = presentDistrictID.value;
        }
    });

});

// Nominee 
let nomineeList = [];
let familyList = [];

document.addEventListener("DOMContentLoaded", function () {

    const dropdown = document.getElementById("esicDropdown");
    const ipBlock = document.getElementById("ipBlock");    

    function toggleIpField() {
        if (dropdown.value === "1") {
            ipBlock.style.display = "block";
        } else {
            ipBlock.style.display = "none";
        }
    }
    toggleIpField();
    dropdown.addEventListener("change", toggleIpField);

    // Nominee 
    window.addNominee = function () {
        const stateDropdown = document.getElementById('nomineeStateDropdown');
        const districtDropdown = document.getElementById('nomineeDistrictDropdown');

        const nominee = {
            name: document.getElementById('name').value,
            dob: document.getElementById('dob').value,
            relationship: document.getElementById('relationship').value,
            address: document.getElementById('address').value,
            stateId: stateDropdown.value,
            stateName: stateDropdown.options[stateDropdown.selectedIndex].text,
            districtId: districtDropdown.value,
            districtName: districtDropdown.selectedIndex > 0 ? districtDropdown.options[districtDropdown.selectedIndex].text : "",
            pincode: document.getElementById('pincode').value,
            isFamilyMember: document.getElementById('isFamilyMember').value
        };

        if (!nominee.name || !nominee.dob || !nominee.relationship || !nominee.address || !nominee.stateId || nominee.stateId === "" ||
            !nominee.districtId || nominee.districtId === "" || !nominee.pincode) {
            alert("Please fill all required fields!!!");
            return;
        }

        nomineeList.push(nominee);
        rebuildNomineeHiddenInputs();
        console.log(nomineeList);       
        renderTable();
        clearFields();
    };


    function rebuildNomineeHiddenInputs() {
        const container = document.getElementById("nomineeHiddenFields");
        container.innerHTML = "";

        nomineeList.forEach((n, i) => {
            container.insertAdjacentHTML("beforeend", `
            <input type="hidden" name="NomineeDetails[${i}].Name" value="${escapeHtml(n.name)}" />
            <input type="hidden" name="NomineeDetails[${i}].Dob" value="${n.dob}" />
            <input type="hidden" name="NomineeDetails[${i}].Relationship" value="${escapeHtml(n.relationship)}" />
            <input type="hidden" name="NomineeDetails[${i}].Address" value="${escapeHtml(n.address)}" />
            <input type="hidden" name="NomineeDetails[${i}].StateId" value="${n.stateId}" />
            <input type="hidden" name="NomineeDetails[${i}].DistrictId" value="${n.districtId}" />
            <input type="hidden" name="NomineeDetails[${i}].Pincode" value="${escapeHtml(n.pincode || "")}" />
            <input type="hidden" name="NomineeDetails[${i}].IsFamilyMember" value="${n.isFamilyMember}" />
        `);
        });
    }

    function renderTable() {
        debugger;
        const tbody = document.querySelector(".nomineeTable tbody");

        if (!tbody) {
            console.error("Table body NOT FOUND!");
            return;
        }

        tbody.innerHTML = "";

        nomineeList.forEach((item, index) => {
            const row = `
                    <tr>
                        <td>${item.name}</td>
                        <td>${formatDateToDDMMYYYY(item.dob)}</td>
                        <td>${item.relationship}</td>
                        <td>${item.address}</td>
                        <td>${item.stateName}</td>
                        <td>${item.districtName}</td>
                        <td>${item.pincode}</td>
                        <td>${item.isFamilyMember}</td>
                        <td>
                            <button type="button" onclick="deleteRow(${index})">Delete</button>
                        </td>
                    </tr>
                `;
            tbody.insertAdjacentHTML("beforeend", row);
        });
    }
    function formatDateToDDMMYYYY(dateStr) {
        if (!dateStr) return "";

        const parts = dateStr.split('-'); // yyyy-mm-dd
        if (parts.length !== 3) return dateStr;

        return `${parts[2]}/${parts[1]}/${parts[0]}`;
    }

    window.deleteRow = function (index) {
        nomineeList.splice(index, 1);
        rebuildNomineeHiddenInputs();
        renderTable();
    };

    // Family

    function validateSelectedFile(file, allowedExtensions, minSizeKB, maxSizeKB, fieldLabel) {
        if (!file) {
            alert(`Please upload ${fieldLabel}`);
            return false;
        }

        const fileName = file.name.toLowerCase();
        const extension = fileName.substring(fileName.lastIndexOf('.'));
        const sizeKB = file.size / 1024;

        if (!allowedExtensions.includes(extension)) {
            alert(`${fieldLabel} must be in ${allowedExtensions.join(", ").replaceAll(".", "").toUpperCase()} format only.`);
            return false;
        }

        if (sizeKB < minSizeKB || sizeKB > maxSizeKB) {
            if (minSizeKB === 0) {
                alert(`${fieldLabel} size must not exceed ${maxSizeKB} KB.`);
            } else {
                alert(`${fieldLabel} size must be between ${minSizeKB} KB to ${maxSizeKB} KB.`);
            }
            return false;
        }

        return true;
    }


    window.addFamily = function () {
        let photoFile = document.getElementById("familyPhoto").files[0];
        let proofFile = document.getElementById("proofFile").files[0];

        const familystateDropdown = document.getElementById('familystate');
        const familydistrictDropdown = document.getElementById('familydistrict');

        let family = {
            name: document.getElementById("familyName").value,
            dob: document.getElementById("familyDob").value,
            relationship: document.getElementById("familyRelationship").value,
            gender: document.getElementById("familyGender").value,
            residing: document.getElementById("familyResiding").value,
            //state: document.getElementById("familystate").value,
            //district: document.getElementById("familydistrict").value,
            state: familystateDropdown.selectedIndex > 0 ? familystateDropdown.value : "",
            stateName: familystateDropdown.selectedIndex > 0 ? familystateDropdown.options[familystateDropdown.selectedIndex].text : "",
            district: familydistrictDropdown.selectedIndex > 0 ? familydistrictDropdown.value : "",
            districtName: familydistrictDropdown.selectedIndex > 0 ? familydistrictDropdown.options[familydistrictDropdown.selectedIndex].text : "",
            photo: photoFile ? photoFile.name : "",
            proofType: document.getElementById("familyProofType").value,
            proof: proofFile ? proofFile.name : "",

            photoFileObj: photoFile,
            proofFileObj: proofFile

        };

        if (!family.name || !family.dob || !family.relationship || !family.gender) {
            alert("Please fill all required fields");
            return;
        }

        if (!photoFile) {
            alert("Please upload photo");
            return;
        }

        if (!family.proofType) {
            alert("Please select proof type");
            return;
        }

        if (!proofFile) {
            alert("Please upload proof file");
            return;
        }

        const isPhotoValid = validateSelectedFile(
            photoFile,
            [".jpg", ".jpeg"],
            50,
            100,
            "Photo"
        );

        if (!isPhotoValid) {
            document.getElementById("familyPhoto").value = "";
            return;
        }

        // Proof document validation: PDF/JPG/JPEG only, max 200 KB
        const isProofValid = validateSelectedFile(
            proofFile,
            [".pdf", ".jpg", ".jpeg"],
            0,
            200,
            "Proof document"
        );

        if (!isProofValid) {
            document.getElementById("proofFile").value = "";
            return;
        }

        familyList.push(family);

        photoDT.items.add(photoFile);
        proofDT.items.add(proofFile);

        document.getElementById("FamilyMemberPhotosInput").files = photoDT.files;
        document.getElementById("FamilyProofDocsInput").files = proofDT.files;

        rebuildFamilyHiddenInputs();
        renderFamilyTable();
        clearFamilyFields();
    };
    function rebuildFamilyHiddenInputs() {
        const container = document.getElementById("familyHiddenFields");
        container.innerHTML = "";

        photoDT = new DataTransfer();
        proofDT = new DataTransfer();

        familyList.forEach((f, i) => {
            container.insertAdjacentHTML("beforeend", `
            <input type="hidden" name="FamilyParticulars[${i}].Name" value="${escapeHtml(f.name)}" />
            <input type="hidden" name="FamilyParticulars[${i}].Dob" value="${f.dob}" />
            <input type="hidden" name="FamilyParticulars[${i}].Relationship" value="${escapeHtml(f.relationship)}" />
            <input type="hidden" name="FamilyParticulars[${i}].Gender" value="${escapeHtml(f.gender)}" />
            <input type="hidden" name="FamilyParticulars[${i}].ResidingWith" value="${f.residing}" />
            <input type="hidden" name="FamilyParticulars[${i}].StateIdofResiding" value="${f.state || ''}" />
            <input type="hidden" name="FamilyParticulars[${i}].DistrictIdofResiding" value="${f.district || ''}" />
            <input type="hidden" name="FamilyParticulars[${i}].StateName" value="${f.stateName}" />
            <input type="hidden" name="FamilyParticulars[${i}].DistrictName" value="${f.districtName}" />
            <input type="hidden" name="FamilyParticulars[${i}].TypeOfProof" value="${f.proofType}" />
        `);
        });
    }    
    function renderFamilyTable() {

        let tbody = document.querySelector("#familyTable tbody");
        tbody.innerHTML = "";

        familyList.forEach((item, index) => {

            let row = `
            <tr>
                <td>${item.name}</td>
                <td>${formatDateToDDMMYYYY(item.dob)}</td>
                <td>${item.relationship}</td>
                <td>${item.gender}</td>
                <td>${item.residing}</td>
                <td>${item.stateName || ''}</td>
                <td>${item.districtName || ''}</td>
                <td>${item.photo}</td>
                <td>${item.proofType}</td>
                <td>${item.proof}</td>
                <td><button onclick="deleteFamilyRow(${index})">Delete</button></td>
            </tr>
        `;

            tbody.insertAdjacentHTML("beforeend", row);
        });
    }

    let photoDT = new DataTransfer();
    let proofDT = new DataTransfer();
    window.deleteFamilyRow = function (index) {
        familyList.splice(index, 1);
        rebuildFamilyHiddenInputs(); 
        renderFamilyTable();
    };

    const form = document.getElementById("employeeForm");
    form.addEventListener("submit", function () {
        rebuildNomineeHiddenInputs();
        rebuildFamilyHiddenInputs();
    });
});


function clearFields() {
    document.getElementById('name').value = "";
    document.getElementById('dob').value = "";
    document.getElementById('relationship').value = "";
    document.getElementById('address').value = "";
    document.getElementById('nomineeStateDropdown').value = "";
   // document.getElementById('nomineeDistrictDropdown').value = "";
    document.getElementById("nomineeDistrictDropdown").innerHTML = '<option value="">-- Select District --</option>';
    document.getElementById('pincode').value = "";
    document.getElementById('isFamilyMember').value = "";
}

document.getElementById("familyResiding").addEventListener("change", function () {

    if (this.value === "No") {
        document.getElementById("familyStateContainer").style.display = "inline";
        document.getElementById("familyDistrictContainer").style.display = "inline";
    } else {
        document.getElementById("familyStateContainer").style.display = "none";
        document.getElementById("familyDistrictContainer").style.display = "none";
    }
});

function escapeHtml(str) {
    return (str || "").replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}
function clearFamilyFields() {   

    document.getElementById("familyName").value = "";
    document.getElementById("familyDob").value = "";
    document.getElementById("familyRelationship").value = "";
    document.getElementById("familyGender").value = "";
    document.getElementById("familyResiding").value = "";
    document.getElementById("familystate").value = "";
    document.getElementById("familydistrict").value = "";
    document.getElementById("familyPhoto").value = "";
    document.getElementById("familyProofType").value = "";
    document.getElementById("proofFile").value = "";

    document.getElementById("familyStateContainer").style.display = "none";
    document.getElementById("familyDistrictContainer").style.display = "none";

}

document.getElementById("AadhaarFile").addEventListener("change", function () {

    if (this.files.length === 0) return;

    const file = this.files[0];
    const fileName = file.name;
    const extension = fileName.split('.').pop().toLowerCase();

    if (extension !== "pdf") {
        alert("Only PDF files are allowed.");

        this.value = "";
        return;
    }
});

document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll(".photo-upload").forEach(function (input) {

        input.addEventListener("change", function () {

            if (this.files.length === 0) return;

            const file = this.files[0];
            const fileName = file.name;
            const extension = fileName.split('.').pop().toLowerCase();

            const allowedExtensions = ["jpg", "jpeg"];

            if (!allowedExtensions.includes(extension)) {
                alert("Only JPG/JPEG files are allowed.");

                this.value = ""; 
                return;
            }

            const fileSizeKB = file.size / 1024;

            if (fileSizeKB < 50 || fileSizeKB > 100) {
                alert("Photo size should be between 50 KB to 100 KB.");
                this.value = "";
                return;
            }

        });

    });

});


document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll(".file-upload").forEach(function (input) {

        input.addEventListener("change", function () {

            if (this.files.length === 0) return;

            const file = this.files[0];
            const fileName = file.name;
            const extension = fileName.split('.').pop().toLowerCase();

            const allowedExtensions = ["pdf", "jpg", "jpeg"];

            if (!allowedExtensions.includes(extension)) {
                alert("Only PDF, JPG, and JPEG files are allowed.");

                this.value = ""; 
                return;
            }

            const fileSizeKB = file.size / 1024;

            if (fileSizeKB > 200) {
                alert("Max size of the document should be 200 KB.");
                this.value = "";
                return;
            }

        });

    });

});


function validateFileInput(input) {

    const file = input.files[0];
  //  const errorSpan = input.nextElementSibling;

    if (!file) return;

    //errorSpan.innerText = "";

    // ✅ Read config from data attributes
    const allowedExtensions = input.getAttribute("data-allowed").split(",");
    const minSizeKB = parseInt(input.getAttribute("data-minsize")); // KB
    const maxSizeKB = parseInt(input.getAttribute("data-maxsize")); // KB

    // ✅ Extension validation
    const fileName = file.name.toLowerCase();
    const fileExtension = fileName.substring(fileName.lastIndexOf('.'));

    if (!allowedExtensions.includes(fileExtension)) {
        alert(`Allowed formats: ${allowedExtensions.join(", ").replaceAll(".", "").toUpperCase()}`);
        //errorSpan.innerText = `Allowed formats: ${allowedExtensions.join(", ")}`;
        input.value = "";
        return;
    }

    // ✅ Size validation
    const fileSizeKB = file.size / 1024;

    if (fileSizeKB < minSizeKB || fileSizeKB > maxSizeKB) {
        alert(`File size must be between: ${minSizeKB}KB to ${maxSizeKB}KB`);
        //errorSpan.innerText =
        //    `File size must be between ${minSizeKB}KB to ${maxSizeKB}KB.`;
        input.value = "";
        return;
    }
}


document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("employeeForm");
    const btnPreview = document.getElementById("btnPreview");
    const btnEdit = document.getElementById("btnEdit");
    const btnFinalSubmit = document.getElementById("btnFinalSubmit");
    const previewBanner = document.getElementById("previewBanner");

    function getControls() {
        return form.querySelectorAll("input, select, textarea, button");
    }

    function togglePreview(enable) {

        getControls().forEach(el => {
            const id = el.id;

            if (
                el.type === "hidden" ||
                id === "btnPreview" ||
                id === "btnEdit" ||
                id === "btnFinalSubmit"
            ) return;

            if (enable) {
                el.setAttribute("disabled", "disabled");
            } else {
                el.removeAttribute("disabled");
            }
        });

        if (enable) {
            form.classList.add("preview-mode");
            previewBanner.style.display = "block";
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

    // Preview
    btnPreview.addEventListener("click", function () {

        if (window.jQuery && $(form).valid) {
            if (!$(form).valid()) return;
        } else {
            if (!form.checkValidity()) {
                form.reportValidity();
                return;
            }
        }

        togglePreview(true);
        window.scrollTo(0, 0);
    });

    // Edit
    btnEdit.addEventListener("click", function () {
        togglePreview(false);
    });

    // Enable all before final submit
    form.addEventListener("submit", function () {
        getControls().forEach(el => {
            if (el.type !== "button") {
                el.removeAttribute("disabled");
            }
        });
    });

});


document.addEventListener("DOMContentLoaded", function () {

    const checkbox = document.getElementById("hasPrevEmployerCheck");
    const section = document.getElementById("prevEmployerSection");

    if (!checkbox || !section) return;

    function clearControl(el) {
        const tag = el.tagName.toLowerCase();
        const type = (el.getAttribute("type") || "").toLowerCase();

        if (type === "hidden") return;

        if (tag === "select") {
            el.value = "";
            el.dispatchEvent(new Event("change", { bubbles: true }));
            return;
        }

        if (tag === "textarea") {
            el.value = "";
            return;
        }

        if (tag === "input") {
            if (type === "checkbox" || type === "radio") {
                el.checked = false;
            } else if (type === "file") {
                el.value = "";
            } else {
                el.value = "";
            }
        }
    }

    function togglePrevEmployerFields() {
        const enabled = checkbox.checked;

        const controls = section.querySelectorAll("input, select, textarea, button");

        controls.forEach(el => {
            if (!enabled) {
                clearControl(el);

                el.classList.remove("input-validation-error");
            }

            el.disabled = !enabled;
        });

       
        if (!enabled) {
            section.querySelectorAll(".field-validation-error, .text-danger")
                .forEach(span => {
                   
                    if (span.hasAttribute("data-valmsg-for") || span.classList.contains("field-validation-error")) {
                        span.textContent = "";
                    }
                });
        }

        section.classList.toggle("is-disabled", !enabled);
    }

    togglePrevEmployerFields();

    checkbox.addEventListener("change", togglePrevEmployerFields);
});


//Reset
function resetForm() {
    location.reload();
}