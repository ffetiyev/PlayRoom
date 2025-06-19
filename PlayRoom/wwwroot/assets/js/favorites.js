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

let likeBtns = document.querySelectorAll(".like");

likeBtns.forEach(btn => {
    btn.addEventListener("click", function () {
        let id = parseInt(this.getAttribute("data-id"));
        let productType = this.getAttribute("product-type");
        let isLiked = btn.classList.contains("liked");

        let url = isLiked
            ? `http://localhost:5125/Favorites/DeleteFromFavorites?id=${id}&type=${productType}`
            : `http://localhost:5125/Favorites/AddToFavorites?id=${id}&type=${productType}`;

        fetch(url, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }
        })
            .then(response => response.text())
            .then(res => {
                document.querySelector(".favorites .favorites-count").innerText = res;

                if (isLiked) {
                    btn.classList.remove("liked");
                } else {
                    btn.classList.add("liked");
                }

                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: isLiked ? 'Sevimlilərdən silindi!' : 'Sevimlilərə əlavə olundu',
                    showConfirmButton: false,
                    timer: 1500,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer);
                        toast.addEventListener('mouseleave', Swal.resumeTimer);
                    }
                });
            });
    });
});
