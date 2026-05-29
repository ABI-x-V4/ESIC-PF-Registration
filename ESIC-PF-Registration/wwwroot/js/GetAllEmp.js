document.addEventListener("DOMContentLoaded", function () {

    const card = document.querySelector(".el-main-card");
    if (!card) return;

    console.log("dataset:", card.dataset);

    let total = parseInt(card.dataset.total || "0", 10);
    let filtered = parseInt(card.dataset.filtered || "0", 10);
    let page = parseInt(card.dataset.page || "1", 10);
    let pageSize = parseInt(card.dataset.pagesize || "5", 10);

    const pgFrom = document.getElementById("pgFrom");
    const pgTo = document.getElementById("pgTo");
    const pgTotal = document.getElementById("pgTotal");
    const pagBtns = document.getElementById("pagBtns");
    const visibleCount = document.getElementById("visibleCount");

    function qsGet(key) {
        const u = new URL(window.location.href);
        return u.searchParams.get(key) || "";
    }

    function qsSet(obj) {
        const u = new URL(window.location.href);
        Object.keys(obj).forEach(k => {
            const v = obj[k];
            if (v === null || v === undefined || v === "") u.searchParams.delete(k);
            else u.searchParams.set(k, v);
        });
        window.location.href = u.pathname + "?" + u.searchParams.toString();
    }

    function updateInfo() {
        if (visibleCount) visibleCount.innerHTML = `<i class="ri-user-line"></i> ${filtered} Records`;
        if (pgTotal) pgTotal.textContent = filtered;

        if (!pgFrom || !pgTo) return;

        if (filtered === 0) {
            pgFrom.textContent = "0";
            pgTo.textContent = "0";
            return;
        }

        const from = ((page - 1) * pageSize) + 1;
        const to = Math.min(page * pageSize, filtered);

        pgFrom.textContent = String(from);
        pgTo.textContent = String(to);
    }

    function renderPagination() {
        if (!pagBtns) return;

        pagBtns.innerHTML = "";
        const totalPages = Math.max(1, Math.ceil(filtered / pageSize));
        if (page > totalPages) page = totalPages;

        const mkBtn = (html, targetPage, disabled = false, active = false) => {
            const b = document.createElement("button");
            b.className = "el-pag-btn" + (active ? " el-pg-active" : "");
            b.disabled = disabled;
            b.innerHTML = html;
            b.onclick = () => qsSet({ page: targetPage });
            return b;
        };

        pagBtns.appendChild(mkBtn(`<i class="ri-arrow-left-s-line"></i>`, page - 1, page <= 1));

        const windowSize = 5;
        let start = Math.max(1, page - Math.floor(windowSize / 2));
        let end = Math.min(totalPages, start + windowSize - 1);
        start = Math.max(1, end - windowSize + 1);

        for (let p = start; p <= end; p++) {
            pagBtns.appendChild(mkBtn(String(p), p, false, p === page));
        }

        pagBtns.appendChild(mkBtn(`<i class="ri-arrow-right-s-line"></i>`, page + 1, page >= totalPages));
    }

    const searchBox = document.getElementById("searchBox");
    if (searchBox) {
        let tmr = null;
        searchBox.addEventListener("input", (e) => {
            clearTimeout(tmr);
            tmr = setTimeout(() => qsSet({ search: e.target.value, page: 1 }), 250);
        });
    }

    const genderSel = document.getElementById("genderSel");
    if (genderSel) {
        genderSel.addEventListener("change", (e) => qsSet({ gender: e.target.value, page: 1 }));
    }

    const statusSel = document.getElementById("statusSel");
    if (statusSel) {
        statusSel.addEventListener("change", (e) => qsSet({ status: e.target.value, page: 1 }));
    }

    const perPageSel = document.getElementById("perPageSel");
    if (perPageSel) {
        perPageSel.addEventListener("change", (e) => qsSet({ pageSize: e.target.value, page: 1 }));
    }

    window.sortCol = function (col) {
        const currentSortBy = qsGet("sortBy") || "Name";
        const currentSortDir = qsGet("sortDir") || "asc";

        let nextDir = "asc";
        if (currentSortBy.toLowerCase() === String(col).toLowerCase()) {
            nextDir = (currentSortDir.toLowerCase() === "asc") ? "desc" : "asc";
        }
        qsSet({ sortBy: col, sortDir: nextDir, page: 1 });
    };

    window.exportCSV = function () {
        alert("CSV export: we can implement server export with current filters/sort.");
    };

    updateInfo();
    renderPagination();

});


function printTableOnly() {
    const table = document.getElementById("empTable");
    if (!table) return;

    // Clone table so we can remove Action column if needed
    const clone = table.cloneNode(true);

    // OPTIONAL: Remove last column (ACTION)
    for (const row of clone.querySelectorAll("tr")) {
        const lastCell = row.lastElementChild;
        if (lastCell) lastCell.remove();
    }

    const html = `
<!doctype html>
<html>
<head>
  <meta charset="utf-8" />
  <title>Employees</title>
  <style>
    @page { size: A4 landscape; margin: 10mm; }
    body { font-family: Arial, sans-serif; color: #111; }
    h2 { margin: 0 0 10px 0; font-size: 14px; }
    table { width: 100%; border-collapse: collapse; table-layout: fixed; }
    th, td { border: 1px solid #999; padding: 6px; font-size: 11px; word-break: break-word; }
    th { background: #f2f2f2; }
  </style>
</head>
<body>
  <h2>Employee List</h2>
  ${clone.outerHTML}
</body>
</html>`;

    const w = window.open("", "_blank", "width=1000,height=700");
    w.document.open();
    w.document.write(html);
    w.document.close();

    // Wait for rendering then print
    w.focus();
    setTimeout(() => {
        w.print();
        w.close();
    }, 300);
}
function exportExcel() {
    const currentUrl = new URL(window.location.href);

    const searchParams = currentUrl.searchParams;

    const exportUrl = '/Employee/ExportEmployeeExcel?' + searchParams.toString();

    window.location.href = exportUrl;
}

