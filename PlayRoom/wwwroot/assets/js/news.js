"use strict"

//let allButtons = document.querySelectorAll("#main-top .pagination-part .pagination a");
//let numberButtons = Array.from(allButtons).slice(1, -1); // Exclude first and last

//numberButtons.forEach(element => {
//  element.addEventListener("click", function(event) {
//    event.preventDefault();

//    let activeNumber = document.querySelector("#main-top .pagination-part .pagination .active");
//    if (activeNumber) {
//      activeNumber.classList.remove("active");
//    }

//    element.classList.add("active");
//  });
//});


let showMoreBtn = document.querySelector(".show-more-btn");

showMoreBtn.addEventListener("click", function () {
    let skip = document.querySelectorAll(".news-all .row .news").length;
    let totalCount = parseInt(this.getAttribute("data-count"));

    fetch("http://localhost:5125/News/ShowMore?skip=" + skip, {
        method: "POST",
    })
        .then(response => response.text())
        .then(res => {
            let row = document.querySelector(".news-all .row");
            row.innerHTML += res;

            let loaded = row.querySelectorAll(".news").length;
            if (loaded >= totalCount) {
                showMoreBtn.classList.add("d-none");
            }
        })
        .catch(err => console.error("Error loading more news:", err));
});

