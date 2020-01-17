// this scropt controls the behavior of the video player
// gets elemets
// uses them in functions
// hooks the functions with event listeners

// getting the video player from dom
const player = document.querySelector('.c-video');
const video = player.querySelector('.video');
const pControls = player.querySelector('.controls');

const progress = pControls.querySelector('.progress');
const progress_bar = progress.querySelector('.progress-bar');
const pToggle = (pControls.querySelector('.Buttons')).querySelector('.toggle');
const vToggle = (pControls.querySelector('.Buttons')).querySelector('.vToggle');
const volRanges = (pControls.querySelector('.Buttons')).querySelector('.player__slider');



// funcs
function togglePlay() {
    video.paused ? video.play() : video.pause();
}

function updateButton() {
    console.log("change play");
    this.paused ? pToggle.classList.replace('fa-pause', 'fa-play') : pToggle.classList.replace('fa-play', 'fa-pause');
}

function updateVolButton(vol) {
    const curr = vToggle.classList[2];
    if (vol == 0)
        vToggle.classList.replace(curr, 'fa-volume-off');
    else if (vol <= 0.5 && vol > 0)
        vToggle.classList.replace(curr, 'fa-volume-down');
    else
        vToggle.classList.replace(curr, 'fa-volume-up');
}

function handleRangeUpdate() {
    console.log("new value of " + this.name + " = " + this.value);
    video[this.name] = this.value;
    updateVolButton(this.value);
}

function handleProgress() {
    const percent = (video.currentTime / video.duration) * 100;
    var val = percent + '%';
    progress_bar.style.width = val;
    //console.log(progress_bar.style.width);
}

function scrub(event) {
    const scrubTime = (event.offsetX / progress.offsetWidth) * video.duration;
    video.currentTime = scrubTime;
    console.log(scrubTime);
}

function toggleMute() {
    if (video.muted == false) {
        this.prevIcon = vToggle.classList[2];
        vToggle.classList.replace(this.prevIcon, 'fa-volume-mute');
        video.muted = true;
    }
    else {
        vToggle.classList.replace('fa-volume-mute', this.prevIcon);
        video.muted = false;
    }
}


function handleKeys(event) {
    switch (event.kecode) {
        case 32:
            togglePlay();
            console.log("space was pressed");
            break;
        default:
    }
}



// flags
let mouseDown = false;


// event listeners
video.addEventListener('click', togglePlay);
video.addEventListener('play', updateButton);
video.addEventListener('pause', updateButton);
video.addEventListener('timeupdate', handleProgress);
//document.addEventListener('keydown', handleKeys);

pToggle.addEventListener('click', togglePlay);

vToggle.addEventListener('click', toggleMute);
volRanges.addEventListener('change', handleRangeUpdate);

progress.addEventListener('click', scrub);
progress.addEventListener('mousemove',(event) => mouseDown && scrub(event));
progress.addEventListener('mousedown', () => mouseDown = true);
progress.addEventListener('mouseup', () => mouseDown = false);