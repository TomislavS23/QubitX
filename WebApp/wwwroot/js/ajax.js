function RefreshProfile(idUser)
{
    $.ajax({
        url: `/Profile/GetProfileData/${idUser}`,
        method: "GET"
    })
        .done((data) => {
            //debugger;
            $("#FirstName").text(data.firstName);
            $("#LastName").text(data.lastName);
            $("#Username").text(data.username);
        });
}

function FillProfileModal()
{
    const firstName = $("#FirstName").text().trim();
    const lastName = $("#LastName").text().trim();
    const username = $("#Username").text().trim();

    $("#FirstNameInput").val(firstName);
    $("#LastNameInput").val(lastName);
    $("#UsernameInput").val(username);
}

function SubmitChanges(idUser)
{
    const profile = {
        firstName: $("#FirstNameInput").val(),
        lastName: $("#LastNameInput").val(),
        username: $("#UsernameInput").val()
    };

    $.ajax({
        url: `/Profile/SaveProfileData/${idUser}`,
        method: "PUT",
        contentType: "application/json",
        data: JSON.stringify(profile)
    })
        .done(() => {
            const modalElement = document.getElementById('staticBackdrop');
            const modal = bootstrap.Modal.getInstance(modalElement);

            if (modal) {
                modal.hide();
            }

            RefreshProfile(idUser);
        })
        .fail(() => {
            alert("ERROR: Could not update profile");
        })
}