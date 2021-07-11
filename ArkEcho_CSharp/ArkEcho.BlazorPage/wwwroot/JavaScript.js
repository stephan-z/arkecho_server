var audio;

function Init(Text) {
    audio = new Audio('https://localhost:5001/api/Music/MusicFile/' + Text);
}

function PlayAudio() {
    audio.play();
}

function PauseAudio() {
    audio.pause();
}