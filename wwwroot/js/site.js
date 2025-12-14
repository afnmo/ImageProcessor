const btnOpen = document.getElementById("btnOpen");
const btnToggle = document.getElementById("btnToggle");
const btnStep = document.getElementById("btnStep");

const imgAuto = document.getElementById("imgAuto");
const imgStep = document.getElementById("imgStep");
const loader = document.getElementById("loading");


let imagesOpen = false;
let intervalId = null;


const token = document.querySelector(
    '#antiForgeryForm input[name="__RequestVerificationToken"]'
).value;


btnOpen.addEventListener("click", async () => {
    if (!imagesOpen) {
        await fetch("/?handler=Restore", {
            method: "POST",
            headers: { "RequestVerificationToken": token }
        });

        imgAuto.src = "/images/luffy1.jpg?" + Date.now();
        imgStep.src = "/images/roger1.jpg?" + Date.now();

        imgAuto.classList.remove("hidden");
        imgStep.classList.remove("hidden");

        btnToggle.disabled = false;
        btnStep.disabled = false;

        btnOpen.textContent = "Close Images";
    } else {
        closeImages();
    }

    imagesOpen = !imagesOpen;
});

btnToggle.addEventListener("click", () => {
    if (intervalId) {
        clearInterval(intervalId);
        intervalId = null;

        loader.classList.add("hidden");

        btnToggle.classList.remove("btn-stop");
        btnToggle.classList.add("btn-start");
        btnToggle.querySelector(".btn-text").textContent = "Start";

        return;
    }

    loader.classList.remove("hidden");

    btnToggle.classList.remove("btn-start");
    btnToggle.classList.add("btn-stop");
    btnToggle.querySelector(".btn-text").textContent = "Stop";

    intervalId = setInterval(async () => {
        const res = await fetch("/?handler=Auto", {
            method: "POST",
            headers: { "RequestVerificationToken": token }
        });

        const data = await res.json();
        imgAuto.src = data.imageUrl;
    }, 500);
});


btnStep.addEventListener("click", async () => {
    const res = await fetch("/?handler=Step", {
        method: "POST",
        headers: { "RequestVerificationToken": token }
    });

    const data = await res.json();
    imgStep.src = data.imageUrl;
});

function closeImages() {
    if (intervalId) {
        clearInterval(intervalId);
        intervalId = null;
        btnToggle.textContent = "Start";
    }

    loader.classList.add("hidden");

    imgAuto.classList.add("hidden");
    imgStep.classList.add("hidden");

    btnToggle.disabled = true;
    btnStep.disabled = true;

    btnOpen.textContent = "Open Images";
}
