$(document).ready(function () {




   

    $(document).on('keyup', '#teachersearch', function () {
        
        

            $.ajax({
                url: "/Teacher/TeacherSearch/ ",
                type: "GET",
                data: {
                    "keyword": $("#teachersearch").val()
                },
                success: function (response) {
                    if ($("#teachersearch").val().length > 0) {
                        
                        $("#MyTeachers div").slice().remove()
                        $("#MyTeachers").append(response)
                    }
                    else {
                        $("#MyTeachers").empty(),
                        $("#MyTeachers").append(response)
                    };


                }
            });

        
        
       
       

    })

    $(document).on('keyup', '#coursesearch', function () {



        $.ajax({
            url: "/Course/SearchCourse/ ",
            type: "GET",
            data: {
                "search": $("#coursesearch").val()
            },
            success: function (response) {
                if ($("#coursesearch").val().length > 0) {
                    
                    $("#MyCourses div").slice().remove()
                    $("#MyCourses").append(response)
                }
                else {
                    $("#MyCourses").empty(),
                        $("#MyCourses").append(response)
                };

            }
        });


    });
    
    $(document).on('keyup', '#eventsearch', function () {



        $.ajax({
            url: "/Event/Searchevent/ ",
            type: "GET",
            data: {
                "search": $("#eventsearch").val()
            },
            success: function (response) {
                if ($("#eventsearch").val().length > 0) {
                   
                    $("#MyEvents div").slice().remove()
                    $("#MyEvents").append(response)
                }
                else {
                    $("#MyEvents").empty(),
                        $("#MyEvents").append(response)
                };

            }
        });


    });
    $(document).on('keyup', '#input-search', function () {
        if ($("#input-search").val().length > 0) {
            $.ajax({
                url: "/Home/SearchGlobal/ ",
                type: "Get",
                data: {
                    "Search": $("#input-search").val()
                },
                success: function (response) {
                    $("#search-form li").slice().remove()
                    $("#search-form").append(response)


                }
            });
        }
        else {
            $("#search-form li").remove()

        }

    })


    


    $(document).on('click', '#Mc-embedded-subscribe2', function () {
        $.ajax({
            url: "/Home/SubScribe/",
            type: "Get",
            data: {
                "email": $("#mce-EMAIL").val(),
            },
            success: function (res) {
                $("#Pnim").empty();
                $("#Pnim").append(res);

            }
        })
    })
    

    $(document).on('click', '.reply-btn', function () {
        $.ajax({
            url: "/Contact/Send/",
            type: "Get",
            data: {
                "message": $("#message").val(),
                "email": $("#email").val(),
                "subject": $("#subject").val(),
                "name": $("#name").val(),
            },
            success: function (res) {
                $("#Pnim").empty();
                $("#Pnim").append(res);
                if ($("#email").val() != null) {
                    $("#message").empty();
                    $("#email").empty();
                    $("#subject").empty();
                    $("#name").empty();
                };
            }
        })
    })
})