

function limpiar(text) {

    text = text.toLowerCase();

    text = text.replace(/\á/g, 'a');
    text = text.replace(/\à/g, 'a');
    text = text.replace(/\ä/g, 'a');
    text = text.replace(/\â/g, 'a');
    text = text.replace(/\å/g, 'a');
    text = text.replace(/\é/g, 'e');
    text = text.replace(/\è/g, 'e');
    text = text.replace(/\ë/g, 'e');
    text = text.replace(/\ê/g, 'e');
    text = text.replace(/\í/g, 'i');
    text = text.replace(/\ì/g, 'i');
    text = text.replace(/\ï/g, 'i');
    text = text.replace(/\î/g, 'i');
    text = text.replace(/\ó/g, 'o');
    text = text.replace(/\ò/g, 'o');
    text = text.replace(/\ö/g, 'o');
    text = text.replace(/\ô/g, 'o');
    text = text.replace(/\ú/g, 'u');
    text = text.replace(/\ù/g, 'u');
    text = text.replace(/\ü/g, 'u');
    text = text.replace(/\û/g, 'u');
    text = text.replace(/\ý/g, 'y');
    text = text.replace(/\ÿ/g, 'y');
    text = text.replace(/\ñ/g, 'n');
    text = text.replace(/\ç/g, 'c');

    return text;
}