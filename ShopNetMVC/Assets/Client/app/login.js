var controller = {
    init: function () {
        controller.registerEvents();
    },
    registerEvents: function () {
        $("#LoginUse").off("click").on("click", function (e) {
            // TODO
            $.ajax({
                type: 'POST',
                url: '/user/login',
                data: {

                },
                dataType: 'json',
                success: function (response) {
                    if (response.result === true) {
                        // TODO
                    } else {
                        console.log('error cmnr!');
                    }
                },
                error: function (response) {
                    console.log(response.message);
                }
            });
        });

        $("#LogoutUse").off("click").on("click", function (e) {
            $.ajax({
                type: 'POST',
                url: '/user/logout',
                dataType: 'json',
                success: function (response) {
                    if (response.result === true) {
                        // TODO
                    } else {
                        console.log('error cmnr!');
                    }
                },
                error: function (response) {
                    console.log(response.message);
                }
            });
        });

        $("#Register").off("click").on("click", function (e) {
            // TODO
            $.ajax({
                type: 'POST',
                url: '/user/register',
                data: {
                    // TODO
                },
                dataType: 'json',
                success: function (response) {
                    if (response.result === true) {
                        // TODO
                    } else {
                        console.log('error cmnr!');
                    }
                },
                error: function (response) {
                    console.log(response.message);
                }
            });
        });
    }
};

controller.init();

