﻿const editsubmit = (Idforodebate) => {
    e.preventDefault();
    const form = document.getElementById("Forodebate" + "FormEdit");
    const formData = new FormData(form);
    axios({
        method: "post",
        url: "Forodebate" + "" + `/Edit`,
        data: formData,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    })
        .then(function (response) {
            document.getElementById("listbody").innerHTML = response.data;
            document.getElementById("editmodal").click();
        })
        .catch(function (response) {
            console.log(response);
        });
};
const modalcreate = () => {
    axios({
        baseURL: "Forodebate" + "" + "/Create",
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
    });
};
const modaledit = (Idforodebate) => {
    axios({
        baseURL: "Forodebate" + "" + "/Edit?" + `Idforodebate=${Idforodebate}`,
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
    });
};
const modaldetails = (Idforodebate) => {
    axios({
        baseURL: "Forodebate" + "" + "/Details?" + `Idforodebate=${Idforodebate}`,
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
    });
};
const modaldelete = (Idforodebate) => {
    axios({
        baseURL: "Forodebate" + "" + "/Delete?" + `Idforodebate=${Idforodebate}`,
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
    });
};
const submitforms = (e, url, id) => {
    e.preventDefault();
    document.getElementById("loader_inv").style.visibility = "visible";

    const form = document.getElementById(id);
    const formData = new FormData(form);
    axios({
        method: "post",
        url: "Forodebate" + "/" + `${url}`,
        data: formData,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    })
        .then(function (response) {
            document.getElementById("listbody").innerHTML = response.data;
            document.getElementById("editmodal").click();
            document.getElementsByClassName("modal-backdrop fade show")[0].className = "";
        })
        .catch(function (response) {
            console.log(response);
        }).finally(() => {
            document.getElementById("loader_inv").style.visibility = "hidden";
            refrestjsfunction();
        });
};
const verifyCheckbox = (id) => {
    var checkBox = document.getElementById(id + "_2");
    var text = document.getElementById(id);
    if (checkBox.checked == true) {
        text.value = "S";
    } else {
        text.value = "N";
    }
}
const refrestjsfunction = () => {
    const refreshjs = document.querySelectorAll("script");
    refreshjs.forEach((item) => {
        var src = item.src;
        item.remove();
        var script,
            scriptTag;
        script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = src;
        scriptTag = document.getElementsByTagName('script')[0];
        scriptTag.parentNode.insertBefore(script, scriptTag);
    })
}