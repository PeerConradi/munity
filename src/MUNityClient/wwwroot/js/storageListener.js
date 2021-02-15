function registerStorageListener(resolutionService) {
    window.addEventListener('storage', () => {
        resolutionService.invokeMethodAsync('StorageHasChanged');
    });
}
