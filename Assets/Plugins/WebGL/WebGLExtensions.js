function CheckWebGLExtensions() {
    var gl = document.createElement('canvas').getContext('webgl');
    var supportsOESTextureFloat = !!gl.getExtension('OES_texture_float');
    var supportsEXTColorBufferFloat = !!gl.getExtension('EXT_color_buffer_float');
    var result = supportsOESTextureFloat + "," + supportsEXTColorBufferFloat;
    return result;
}
