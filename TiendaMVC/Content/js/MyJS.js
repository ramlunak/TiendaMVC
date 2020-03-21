
function SelectOnchange(selectObj) {
    var selectIndex = selectObj.selectedIndex;
    var iso2 = selectObj.options[selectIndex].id.toLowerCase();
    var prefijo = selectObj.options[selectIndex].value;

    document.getElementById('bandera').src = src = "/Images/Paises/" + iso2 + ".png";
    document.getElementById('prefijo').innerHTML ="+" + prefijo;

}