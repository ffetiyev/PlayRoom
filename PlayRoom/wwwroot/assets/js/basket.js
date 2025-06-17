"use strict"
//var QtyInput = (function () {
//	var $qtyInputs = $(".qty-input");

//	if (!$qtyInputs.length) {
//		return;
//	}

//	var $inputs = $qtyInputs.find(".product-qty");
//	var $countBtn = $qtyInputs.find(".qty-count");
//	var qtyMin = parseInt($inputs.attr("min"));
//	var qtyMax = parseInt($inputs.attr("max"));

//	$inputs.change(function () {
//		var $this = $(this);
//		var $minusBtn = $this.siblings(".qty-count--minus");
//		var $addBtn = $this.siblings(".qty-count--add");
//		var qty = parseInt($this.val());

//		if (isNaN(qty) || qty <= qtyMin) {
//			$this.val(qtyMin);
//			$minusBtn.attr("disabled", true);
//		} else {
//			$minusBtn.attr("disabled", false);

//			if (qty >= qtyMax) {
//				$this.val(qtyMax);
//				$addBtn.attr('disabled', true);
//			} else {
//				$this.val(qty);
//				$addBtn.attr('disabled', false);
//			}
//		}
//	});

//	$countBtn.click(function () {
//		var operator = this.dataset.action;
//		var $this = $(this);
//		var $input = $this.siblings(".product-qty");
//		var qty = parseInt($input.val());

//		if (operator == "add") {
//			qty += 1;
//			if (qty >= qtyMin + 1) {
//				$this.siblings(".qty-count--minus").attr("disabled", false);
//			}

//			if (qty >= qtyMax) {
//				$this.attr("disabled", true);
//			}
//		} else {
//			qty = qty <= qtyMin ? qtyMin : (qty -= 1);

//			if (qty == qtyMin) {
//				$this.attr("disabled", true);
//			}

//			if (qty < qtyMax) {
//				$this.siblings(".qty-count--add").attr("disabled", false);
//			}
//		}

//		$input.val(qty);
//	});
//})();

// Toggle promo code section

let addPromoCodeBtn = document.querySelector("#main-top .basket-main .promocode .main-part .add-promo-item .add");
let removePromoCodeBtn = addPromoCodeBtn.nextElementSibling;
let promocodeSection = document.querySelector("#main-top .basket-main .promocode .add-promocode");

addPromoCodeBtn.addEventListener("click", function () {
    promocodeSection.classList.remove("d-none");
    addPromoCodeBtn.classList.add("d-none");
    removePromoCodeBtn.classList.remove("d-none");
});

removePromoCodeBtn.addEventListener("click", function () {
    promocodeSection.classList.add("d-none");
    addPromoCodeBtn.classList.remove("d-none");
    removePromoCodeBtn.classList.add("d-none");
});

// Handle promo code submission
let applyButton = document.querySelector(".promocode .confirm-code button");
let promoInput = document.querySelector(".promocode .promo-input");

applyButton.addEventListener("click", async function () {
    const code = promoInput.value.trim();

    if (!code) {
        alert("Zəhmət olmasa promokod daxil edin.");
        return;
    }

    // Example API call — replace '/api/apply-promo' with your actual endpoint
    const response = await fetch("/api/apply-promo", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ promoCode: code })
    });

    const result = await response.json();

    if (result.success) {
        alert(`Promokod tətbiq olundu: ${result.discount}% endirim`);
        // You can update price/total here
    } else {
        alert("Promokod etibarsızdır.");
    }
});

let deleteBasketBtns = document.querySelectorAll(".basket-delete-btn");

deleteBasketBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let id = parseInt(this.getAttribute("data-id"));
        let productType = this.getAttribute("product-type");

        fetch(`http://localhost:5125/Cart/Delete?id=${id}&type=${productType}`, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }
        })
            .then(response => response.json()).then(res => {
                this.parentNode.parentNode.parentNode.parentNode.remove();
                debugger;
                document.querySelector(".total-price .total-discountless").innerText = `${Math.round(res[2])} ₼`;
                document.querySelector(".discount .dicount-value").innerText = `${Math.round(res[3])} ₼`;
                document.querySelector(".final-price .res-price").innerText = `${Math.round(res[1])} ₼`;
                document.querySelector(".basket .basket-link .basket-count").innerText = res[0];
                document.querySelector(".basket-count span").innerText = `Səbətim (${res[0]})`;
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: 'Məhsul səbətdən silindi!',
                    showConfirmButton: false,
                    timer: 1500,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                });

            })
    })

}))


document.querySelectorAll(".qty-input .qty-count--add").forEach(btn => {
    btn.addEventListener("click", function () {
        let parent = this.closest(".qty-input");
        let id = parseInt(parent.getAttribute("data-id"));
        let productType = parent.getAttribute("product-type");
        let input = parent.querySelector(".product-qty");

        let currentValue = parseInt(input.value);
        let maxValue = parseInt(input.max);

        if (currentValue < maxValue) {
            input.value = currentValue + 1;

            fetch(`http://localhost:5125/Cart/IncreaseProductCount?id=${id}&type=${productType}`, {
                method: "POST",
                headers: {
                    "Content-type": "application/json; charset=UTF-8"
                }
            })
                .then(response => response.json())
                .then(res => {
                    document.querySelector(".total-price .total-discountless").innerText = `${Math.round(res[2])} ₼`;
                    document.querySelector(".discount .dicount-value").innerText = `${Math.round(res[3])} ₼`;
                    document.querySelector(".final-price .res-price").innerText = `${Math.round(res[1])} ₼`;
                    document.querySelector(".basket .basket-link .basket-count").innerText = res[0];
                    document.querySelector(".basket-count span").innerText = `Səbətim (${res[0]})`;
                });
        }
    });
});


document.querySelectorAll(".qty-input .qty-count--minus").forEach(btn => {
    btn.addEventListener("click", function () {
        let parent = this.closest(".qty-input");
        let id = parseInt(parent.getAttribute("data-id"));
        let productType = parent.getAttribute("product-type");
        let input = parent.querySelector(".product-qty");

        let currentValue = parseInt(input.value);
        let minValue = 1;

        if (currentValue > minValue) {
            input.value = currentValue - 1;

            fetch(`http://localhost:5125/Cart/DecreaseProductCount?id=${id}&type=${productType}`, {
                method: "POST",
                headers: {
                    "Content-type": "application/json; charset=UTF-8"
                }
            })
                .then(response => response.json())
                .then(res => {
                    document.querySelector(".total-price .total-discountless").innerText = `${Math.round(res[2])} ₼`;
                    document.querySelector(".discount .dicount-value").innerText = `${Math.round(res[3])} ₼`;
                    document.querySelector(".final-price .res-price").innerText = `${Math.round(res[1])} ₼`;
                    document.querySelector(".basket .basket-link .basket-count").innerText = res[0];
                    document.querySelector(".basket-count span").innerText = `Səbətim (${res[0]})`;
                });
        }
    });
});