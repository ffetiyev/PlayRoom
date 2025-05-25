"use strict"


let allButtons = document.querySelectorAll("#main-top .pagination-part .pagination a");
let numberButtons = Array.from(allButtons).slice(1, -1); // Exclude first and last

numberButtons.forEach(element => {
  element.addEventListener("click", function(event) {
    event.preventDefault();

    let activeNumber = document.querySelector("#main-top .pagination-part .pagination .active");
    if (activeNumber) {
      activeNumber.classList.remove("active");
    }

    element.classList.add("active");
  });
});

