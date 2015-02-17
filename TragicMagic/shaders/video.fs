// Defines
const int COLOURS = 10;
vec4 Colour[COLOURS] = {
    vec4( 1.0, 0.0, 1.0, 1.0 ),
    vec4( 1.0, 0.0, 1.0, 1.0 ),
    vec4( 1.0, 0.0, 1.0, 1.0 ),
    vec4( 1.0, 1.0, 1.0, 1.0 ),
    vec4( 0.0, 1.0, 0.0, 1.0 ),
    vec4( 0.1, 0.1, 0.1, 1.0 ),
    vec4( 0.1, 0.1, 0.1, 1.0 ),
    vec4( 0.1, 0.1, 0.1, 1.0 ),
    vec4( 0.1, 0.1, 0.1, 1.0 ),
    vec4( 0.1, 0.1, 0.1, 1.0 ),
};
const int SCALE = 10;

// Time variable passed in by the program
uniform float Time;

// Return a random float between 0 and 1
// IN: (co) The texture coordinate to lookup
// OUT: (float) The random number between 0 and 1
float rand( vec2 co )
{
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main( void )
{
    // Scale down the texture coordinates for bigger pixels
    float scale = 1 / SCALE; // Work out the inverse scale for subtracting as a vector
    vec2 coord = round( ( gl_TexCoord[0] - vec2( scale ) ) * SCALE ) * Time;

    // Get random 0 -> 1
    float random = rand( coord );

    // Scale random from 0 -> COLOURS & round
    int colour = round( random * COLOURS );

    // Set colour
	gl_FragColor = Colour[colour];
}