console.log("home index.js obecny");

Shared.Slider.Init();

$(document).ready(() => 
{
    Shared.Slider.AutoSlide();
    
    $("#prevBtn").click(() => Shared.Slider.skipSlide = true);
    $("#nextBtn").click(() => Shared.Slider.skipSlide = true);
    $(".dot").click(() => Shared.Slider.skipSlide = true);
});
