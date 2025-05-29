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


let specialBannerDeleteBtns = document.querySelectorAll(".banner-delete-btn");

specialBannerDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let bannerId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/SpecialGameBanner/Delete?id=" + bannerId, {

            method: "POST",

        })
            .then(async (response) => {
                if (!response.ok) {
                    let errorText = await response.text();
                    alert(errorText);
                    return;
                }
                this.parentNode.parentNode.remove()
            })
    })

}))

let setActiveBannerImageBtn = document.querySelector(".set-active-btn");

if (setActiveBannerImageBtn) {
    setActiveBannerImageBtn.addEventListener("click", function () {
        let button = this;
        let container = button.parentNode;
        let bannerId = parseInt(container.getAttribute("data-id"));

        fetch("http://localhost:5125/Admin/SpecialGameBanner/SetActiveBanner?id=" + bannerId, {
            method: "POST"
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("Failed to activate banner.");
                }
                return response.text();
            })
            .then(() => {
                // Replace the button with the green "Active" badge
                container.innerHTML = `
                <div style="width: 90px; background-color: limegreen; color: white; border-radius: 7px; padding: 5px 0px 5px 20px;" class="mt-3 d-block">
                    Active
                </div>`;
            })
            .catch(err => {
                console.error(err);
                alert("Failed to set active. Please try again.");
            });
    });
}
