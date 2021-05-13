const DANGER_CLASS = 'text-danger';

let speakerInterval;
let questionInterval;

function startSpeakerTimer(durationInSeconds, elementId) {
    const domElement = document.getElementById(elementId)
    if (!domElement)
        console.log("Element could not be found. Make sure to call after DOM is loaded");
    if (speakerInterval) {
        clearInterval(speakerInterval);
    }

    updateElement(domElement, durationInSeconds);

    let passedSeconds = 0
    speakerInterval = setInterval(() => {
        const diff = durationInSeconds - passedSeconds++
        updateElement(domElement, diff)
    }, 1000);
}

function startQuestionTimer(durationInSeconds, elementId) {
    const domElement = document.getElementById(elementId)
    if (!domElement)
        console.log("Element could not be found. Make sure to call after DOM is loaded");
    if (speakerInterval) {
        clearInterval(questionInterval);
    }

    updateElement(domElement, durationInSeconds);

    let passedSeconds = 0
    questionInterval = setInterval(() => {
        const diff = durationInSeconds - passedSeconds++
        updateElement(domElement, diff)
    }, 1000);
}

function pauseSpeakerTimer(pausedAtInSeconds, elementId) {
    const domElement = document.getElementById(elementId)
    if (!domElement) throw "Element could not be found. Make sure to call after DOM is loaded"
    if (speakerInterval) clearInterval(speakerInterval)
    updateElement(domElement, pausedAtInSeconds)
}

function pauseQuestionTimer(pausedAtInSeconds, elementId) {
    const domElement = document.getElementById(elementId)
    if (!domElement) throw "Element could not be found. Make sure to call after DOM is loaded"
    if (questionInterval) clearInterval(questionInterval)
    updateElement(domElement, pausedAtInSeconds)
}

function resetSpeakerTimer(resetTo, elementId) {
    pauseSpeakerTimer(resetTo, elementId)
}

function resetQuestionTimer(resetTo, elementId) {
    pauseQuestionTimer(resetTo, elementId)
}

function updateElement(domElement, currentTimeInSeconds) {
    if (currentTimeInSeconds < 10) {
        domElement.classList.add(DANGER_CLASS)
    } else {
        if (domElement.classList.contains(DANGER_CLASS)) domElement.classList.remove(DANGER_CLASS)
    }

    if (currentTimeInSeconds <= 0) {
        domElement.innerHTML = 'Bitte zum Ende kommen'
        domElement.classList.add('animate__animated')
        domElement.classList.add('animate__flash')
        //domElement.classList.add('animate__animated')
    } else {
        if (domElement.classList.contains('animate__animated')) domElement.classList.remove('animate__animated')
        if (domElement.classList.contains('animate__flash')) domElement.classList.remove('animate__flash')
        //if (domElement.classList.contains('animate__animated')) domElement.classList.remove('animate__animated')
        domElement.innerHTML = computeTimeString(currentTimeInSeconds)
    }
}

function computeTimeString(seconds) {
    return Math.floor(seconds / 60).toLocaleString("de-DE", {
        minimumIntegerDigits: 2,
    }) + ":" + (seconds % 60).toLocaleString("de-DE", {
        minimumIntegerDigits: 2,
    })
}