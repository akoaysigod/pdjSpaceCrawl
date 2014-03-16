function createShields() {
	%shields = new Sprite( ShipShields ) {
		class = "Shields";
		image = "gameModule:shieldsActive";
		size = "10 10";
		position = Ship.getPosition();
	};
	%shields.setBodyType( dynamic );
	%shields.createCircleCollisionShape( 5 );
	%shields.setCollisionLayers( 2, 12, 13 );
	%shields.setCollisionGroups( 2, 12, 13 );
	%shields.setUpdateCallback( true );
	%shields.setCollisionCallback( true );

	GameScene.add( %shields );
}

function Shields::onUpdate( %this ) {
	%angle = Ship.getAngle();

	if ( %angle < 0 ) {
		%angle *= -1;
	} else if ( %angle > 0 ) {
			%angle = 360 - %angle;
	}

	%t = Vector2Direction( %angle, 1 );
	%modX = getWord( %t, 0 );
	%modY = getWord( %t, 1 );

	%x = getWord( Ship.getPosition(), 0 ) + ( 1 * %modX );
	%y = getWord( Ship.getPosition(), 1 ) + ( 1 * %modY );

	%this.position = %x SPC %y;

	%this.setAngularVelocity( 50 );
}

function Shields::onCollision( %this, %collides, %details ) {
	%change = -1000;
	if ( %collides.name $= "enemyBullet" ) {
		%change = %collides.damage / 2;
		%collides.safeDelete();
	}

	HealthBar.updateHealth( %change );
}