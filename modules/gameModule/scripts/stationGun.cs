function createStationGun( %flip ) {
	%gun = new Sprite( StationGun );
	%gun.name = "stationGun";
	%gun.Size = "5 5";
	%gun.Image = "gameModule:stationGun";
	%gun.setFixedAngle( true );
	%gun.setBodyType( static );

	if ( %flip == 6 ) {
		%gun.setFlipY( true );
	}

	%gun.createPolygonBoxCollisionShape();
	%gun.setCollisionLayers( 0, 1, 2, 7 );
	%gun.setCollisionGroups( 0, 1, 2, 7 );
	%gun.setSceneGroup( 12 );

	%gun.setUpdateCallback( true );

	%gun.health = 7;
	%gun.fireRate = 800;

	return %gun;
}

function StationGun::takeDamage( %this, %dmg ) {
	%this.health -= %dmg; 
	if ( %this.health <= 0 ) {
		%this.stopTimer();
		%this.safeDelete();
	}
}

function StationGun::fireShot( %this ) {
	%bullet = new Sprite( BulletCall );
	%bullet.Name = "enemyBullet";
	%bullet.Image = "gameModule:enemyBullet";
	%bullet.Size = "2 2";
	%bullet.createCircleCollisionShape( 2 );
	%bullet.setCollisionGroups( 20, 30 );
	%bullet.setCollisionLayers( 20, 30 );
	%bullet.setSceneGroup( 13 );
	%bullet.sceneLayer = 10;
	%bullet.Position = %this.getPosition();
	%bullet.setCollisionCallback( true );
	%bullet.setLifeTime( 600 );

	%bullet.damage = 3 + Window.planetID;

	%shootDir = Vector2AngleToPoint( Ship.getPosition(), %this.getPosition() );


	if ( %shootDir < 0 ) {
			%shootDir *= -1;
	} else if ( %shootDir > 0 ) {
			%shootDir = 360 - %shootDir;
	}

	%shootVel = Vector2Direction( %shootDir, 30 );

	GameScene.add( %bullet );

	%bullet.setLinearVelocity( %shootVel );

	alxPlay( "gameModule:enemyLaserSound" );
} 

function StationGun::onUpdate( %this ) {
	%dist = Vector2Distance( %this.getPosition(), Ship.getPosition() );
	%y = getWord( Ship.getPosition(), 1 );

	if ( !%this.getFlipY() ) {
		if ( %y > getWord( %this.getPosition(), 1 ) && %dist < 40) {
			if ( !%this.isTimerActive() ) {
				%this.startTimer( fireShot, %this.fireRate, 0 );
			}
		} else {
			%this.stopTimer();
		}
	} else if ( %this.getFlipY() ) {
		if ( %y > getWord( %this.getPosition(), 1 ) && %dist < 40 ) {
			if ( !%this.isTimerActive() ) {
				%this.startTimer( fireShot, %this.fireRate, 0 );
			}
		} else {
			%this.stopTimer();
		}
	}
}