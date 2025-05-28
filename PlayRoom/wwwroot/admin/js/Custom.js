"use strict"

let deleteBtns = document.querySelectorAll(".delete-btn");

deleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let productId = parseInt(this.getAttribute("data-id"));
      fetch("http://localhost:5125/Admin/Discount/Delete?id=" + productId,{

        method:"POST",

        })
        .then(response => response.text()).then(res => {
            this.parentNode.parentNode.remove()
            })
        })

}))
