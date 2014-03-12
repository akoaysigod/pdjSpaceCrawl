function createStationGun( %flip ) {
	%gun = new Sprite();
	%gun.Size = "5 5";
	%gun.Image = "gameModule:stationGun";

	if ( %flip == 6 ) {
		%gun.setFlipY( true );
	}

	return %gun;
}