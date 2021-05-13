const DANGER_CLASS = 'text-danger';

let speakerInterval;
let questionInterval;

function startSpeakerTimer(durationInSeconds, classSelector) {
    if (speakerInterval) {
        clearInterval(speakerInterval);
    }

    updateElements(classSelector, durationInSeconds);

    let passedSeconds = 0
    speakerInterval = setInterval(() => {
        const diff = durationInSeconds - passedSeconds++
        updateElements(classSelector, diff)
    }, 1000);
}

function startQuestionTimer(durationInSeconds, classSelector) {
    if (speakerInterval) {
        clearInterval(questionInterval);
    }

    updateElements(classSelector, durationInSeconds);

    let passedSeconds = 0
    questionInterval = setInterval(() => {
        const diff = durationInSeconds - passedSeconds++
        updateElements(classSelector, diff)
    }, 1000);
}

function pauseSpeakerTimer(pausedAtInSeconds, elementId) {
    if (speakerInterval) clearInterval(speakerInterval)
    updateElements(elementId, pausedAtInSeconds)
}

function pauseQuestionTimer(pausedAtInSeconds, elementId) {
    if (questionInterval) clearInterval(questionInterval)
    updateElements(elementId, pausedAtInSeconds)
}

function resetSpeakerTimer(resetTo, elementId) {
    pauseSpeakerTimer(resetTo, elementId)
}

function resetQuestionTimer(resetTo, elementId) {
    pauseQuestionTimer(resetTo, elementId)
}

function updateElements(identifier, currentTimeInSeconds) {
    const elements = fetchElements(identifier);
    if (currentTimeInSeconds < 10) {
        addClasses(elements, [DANGER_CLASS])
    } else {
        removeClasses(elements, [DANGER_CLASS])
    }

    if (currentTimeInSeconds <= 0) {
        elements.forEach(e => e.innerHTML = 'Bitte zum Ende kommen')
        addClasses(elements, ['animate__animated', 'animate__flash'])
    } else {
        const timeStr = computeTimeString(currentTimeInSeconds)
        elements.forEach(e => e.innerHTML = timeStr)
        removeClasses(elements, ['animate__animated', 'animate__flash'])
    }
}

function addClasses(DOMElements, classes) {
    if (!DOMElements || DOMElements.length == 0 || !classes || classes.length == 0)
        return;

    DOMElements.forEach(n => {
        classes.forEach(cl => {
            if (!n.classList?.contains(cl))
                n.classList.add(cl);
        });

    });
}

function removeClasses(DOMElements, classes) {

    if (!DOMElements || DOMElements.length == 0 || !classes || classes.length == 0)
        return;

    DOMElements.forEach(n => {
        classes.forEach(cl => {
            if (n.classList?.contains(cl))
                n.classList.remove(cl);
        });

    });
}

function computeTimeString(seconds) {
    return Math.floor(seconds / 60).toLocaleString("de-DE", {
        minimumIntegerDigits: 2,
    }) + ":" + (seconds % 60).toLocaleString("de-DE", {
        minimumIntegerDigits: 2,
    })
}

function fetchElements(identifier) {
    return [...document.getElementsByClassName(identifier)]
}