<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p" crossorigin="anonymous"></script>

var isLoadingVisible = false;

function showLoader() {
    if (!isLoadingVisible) {
        $("div#loader").fadeIn("fast");
    }
};

function hideLoader() {
    if (isLoadingVisible) {
        var loader = $("div#loader");

        loader.stop();
        loader.fadeOut();
        isLoadingVisible = false;
    }
};