"use strict"

let languageChanger = document.querySelector("body .navbar .language .main-language");

languageChanger.addEventListener("click",function(){
    let allLanguages = document.querySelector("body .navbar .language .all-languages");
    if(allLanguages.classList.contains("d-none")){
        allLanguages.classList.remove("d-none");
    }
    else{
        allLanguages.classList.add("d-none");
    }
    
})

let searchButton = document.querySelector("body .navbar .search-like-basket .search");

searchButton.addEventListener("click",function(){
    let searchTab = document.querySelector("body .navbar .search-form");
    if(searchTab.classList.contains("d-none")){
        searchTab.classList.remove("d-none");
    }
    else{
        searchTab.classList.add("d-none");
    }
    
})

var productLikeButton = document.querySelectorAll(" .products .product .card-main .card-top .like");

productLikeButton.forEach(element => {
    element.addEventListener("click",function(){
    if(element.style.color==='red'){
        element.style.color='gray'
    }
    else {
         element.style.color='red'
    }
})
});

window.addEventListener('DOMContentLoaded', () => {
    const list = document.querySelector('#companies .list');
    const companies = list.querySelectorAll('.company');
    const companyCount = companies.length;

    const durationPerCompany = 4; // seconds per company
    const bufferSeconds = 5;
    const totalDuration = companyCount * durationPerCompany + bufferSeconds;

    list.style.animationDuration = `${totalDuration}s`;
});


//sidebar

(function() {
  var body = document.body;
  var burgerMenu = document.querySelector('.b-menu');
  var burgerContain = document.getElementsByClassName('b-container')[0];
  var burgerNav = document.getElementsByClassName('b-nav')[0];
  burgerMenu.addEventListener('click', function toggleClasses() {
    [body, burgerContain, burgerNav].forEach(function (el) {

      el.classList.toggle('open');
    });
  }, false);
})();

const burger = document.querySelector('.b-menu');
const sidebar = document.querySelector('.sidebar');
const overlay = document.querySelector('.sidebar-overlay');

burger.addEventListener('click', () => {
  sidebar.classList.toggle('open');

  if (sidebar.classList.contains('open')) {
    document.body.classList.add('no-scroll');
  } else {
    document.body.classList.remove('no-scroll');
  }
});

let addBasketBtns = document.querySelectorAll(".add-basket-btn");

addBasketBtns.forEach(btn => {
    btn.addEventListener("click", function () {
        let id = parseInt(this.getAttribute("data-id"));
        let productType = this.getAttribute("product-type");

        fetch(`http://localhost:5125/Home/AddProductToBasket?id=${id}&productType=${productType}`, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }
        })
            .then(response => response.text())
            .then(res => {
                document.querySelector(".basket .basket-link .basket-count").innerText = res;

                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: 'Səbətə əlavə olundu',
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




