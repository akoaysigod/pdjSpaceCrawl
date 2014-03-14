if ( !isObject( ShipControls ) ) { 
	%template = new BehaviorTemplate( ShipControls );
	%template.friendlyName = "Controls";
	%template.behaviorType = "Input";
	%template.description = "player controls";
	
	%template.addBehaviorField( upKey, "thrust", keybind, "keyboard W" );
	//%template.addBehaviorField( downKey, "Key to bind to downward movement", keybind, "keyboard S" );
	%template.addBehaviorField( leftKey, "rotate left", keybind, "keyboard A" );
	%template.addBehaviorField( rightKey, "rotate right", keybind, "keyboard D" );
	%template.addBehaviorField( spaceKey, "shoot", keybind, "keyboard space" );
	%template.addBehaviorField( escapeKey, "pause game", keybind, "keyboard escape" );
}

function ShipControls::onBehaviorAdd( %this ) {  
	if ( !isObject( GlobalActionMap ) ) 
		return;
		
	GlobalActionMap.bindObj( getWord( %this.upKey, 0 ), getWord( %this.upKey, 1 ), "accelerate", %this );
	//GlobalActionMap.bindObj( getWord( %this.downKey, 0 ), getWord( %this.downKey, 1 ), "moveDown", %this );
	GlobalActionMap.bindObj( getWord( %this.leftKey, 0 ), getWord( %this.leftKey, 1 ), "turnLeft", %this );
 	GlobalActionMap.bindObj( getWord( %this.rightKey, 0 ), getWord( %this.rightKey, 1 ), "turnRight", %this );
 	GlobalActionMap.bindObj( getWord( %this.spaceKey, 0 ), getWord( %this.spaceKey, 1 ), "shoot", %this );
 	GlobalActionMap.bindObj( getWord( %this.escapeKey, 0 ), getWord( %this.escapeKey, 1) , "pauseGame", %this );
 	
 	%this.maxSpeed = 35;
 	%this.isAccel = false;
}

function ShipControls::onBehaviorRemove( %this ) {
	if ( !isObject( GlobalActionMap ) )
		return;
		
	GlobalActionMap.unbindObj( getWord( %this.upKey, 0 ), getWord( %this.upKey, 1 ), %this );
	GlobalActionMap.unbindObj( getWord( %this.downKey, 0 ), getWord( %this.downKey, 1 ), %this );
	GlobalActionMap.unbindObj( getWord( %this.leftKey, 0 ), getWord( %this.leftKey, 1 ), %this );
	GlobalActionMap.unbindObj( getWord( %this.rightKey, 0 ), getWord( %this.rightKey, 1 ), %this );
	GlobalActionMap.unbindObj( getWord( %this.spaceKey, 0 ), getWord( %this.spaceKey, 1 ), %this );
	GlobalActionMap.unbindObj( getWord( %this.escapeKey, 0 ), getWord( %this.escapeKey, 1), %this );
}

function ShipControls::pauseGame( %this, %val ) {
	if ( !isObject( MessageWindow ) ) {
		if ( %val == 0 ) {
			if ( $pauseStatus ) {
				unpause();
				$pauseStatus = false;
			} else { 
				pause();
				$pauseStatus = true;
			}
		}
	}
}

function ShipControls::accelerate( %this, %val ) { 
	if ( $pauseStatus ) {
		return;
	}

	if ( !%val ) {
		%this.isAccel = false;
		%this.stopThrust();
		return;
	}
	
	%adjustedAngle = %this.owner.Angle;
	
	if ( %adjustedAngle < 0 ) {
			%adjustedAngle *= -1;
	} else if ( %adjustedAngle > 0 ) {
			%adjustedAngle = 360 - %adjustedAngle;
	}
	
	if ( %this.isAccel ) { 
			%thrustVector = Vector2Direction( %adjustedAngle, 30 );
	} else {
			%thrustVector = Vector2Direction( %adjustedAngle, 30 );
			%this.owner.setLinearDamping( 0.0 );
			%this.owner.setAngularDamping( 2.0 );
	}
	%this.isAccel = true;
	
	%x = %this.owner.getLinearVelocityX();
	%y = %this.owner.getLinearVelocityY();
	if ( mAbs( %x ) > %this.maxSpeed ) {
		%thrustVector = "0" SPC getWord( %thrustVector, 1 );
		if ( %x > 0 ) {
			%this.owner.setLinearVelocityX( %this.maxSpeed - 1 );
		} else { 
			%this.owner.setLinearVelocityX( -1 * ( %this.maxSpeed - 1 ));
		}
	}
	if ( mAbs( %y ) > %this.maxSpeed ) {
		%thrustVector = getWord( %thrustVector, 0 ) SPC "0";
		if ( %y > 0 ) {
			%this.owner.setLinearVelocityY( %this.maxSpeed - 1 );
		} else { 
			%this.owner.setLinearVelocityY( -1 * ( %this.maxSpeed - 1 ) );
		}
	}
	
	%this.owner.applyLinearImpulse( %thrustVector, "0 0" );
	
	if ( %this.owner.isStaticFrameProvider() ) {
		%this.owner.Animation = "gameModule:ShipAnim";
	}
	%this.thrustSchedule = %this.schedule( 100, accelerate, 1 );
}

function ShipControls::turnLeft( %this, %val ) { 
	if ( $pauseStatus ) {
		return;
	}
	
	if ( !%val ) {
		%this.stopTurn();
		return;
	}
	%this.owner.setAngularVelocity( %this.owner.getAngularVelocity() + 20 );
	%this.turnSchedule = %this.schedule( 100, turnLeft, 1 );
}

function ShipControls::turnRight( %this, %val ) {
	if ( $pauseStatus ) {
		return;
	}
	
	if ( !%val ) {
		%this.stopTurn();
		return;
	}
	%this.owner.setAngularVelocity( %this.owner.getAngularVelocity() - 20 );
	%this.turnSchedule = %this.schedule( 100, turnRight, 1 );
}

function ShipControls::stopTurn( %this ) { 
	%this.owner.setAngularVelocity( 0 );
	cancel( %this.turnSchedule );
}

function ShipControls::stopThrust( %this ) {
	%this.owner.setLinearDamping( 0.8 );
	%this.owner.setAngularDamping( 0.0 );
	
	cancel( %this.thrustSchedule );
	
	%this.owner.stopAnimation();
	%this.owner.Image = "gameModule:SpaceShip";
}




function Bullet::onCollision( %this, %collides, %collisionDetails ) {
	%collides.setLinearVelocity( "0 0" );
	
	if ( %collides.name $= "enemy" ) {
		%collides.takeDamage( %this.damage );
	} 

	%this.safeDelete();
}

function ShipControls::bulletTimer( %this ) {
	%adjustedAngle = %this.owner.Angle;
	
	%bullet = new Sprite( Bullet );
	%bullet.setImage( "gameModule:bullet" );
	%bullet.SceneLayer = 10;
	%bullet.setSize( "1 3" );
	%bullet.setAngle( %adjustedAngle );
	%bullet.setBodyType( dynamic );
	%bullet.setCollisionCallback( true );
	%bullet.createPolygonBoxCollisionShape();
	%bullet.setCollisionGroups( 1, 10, 11, 12 );
	%bullet.setSceneLayer( 2 );
	%bullet.setLifetime( 10.0 );

	%bullet.damage = 1;
	
	GameScene.add( %bullet );

	if ( %adjustedAngle < 0 ) {
			%adjustedAngle *= -1;
	} else if ( %adjustedAngle > 0 ) {
			%adjustedAngle = 360 - %adjustedAngle;
	}
	
	
	%shootDir = Vector2Direction( %adjustedAngle, 50 );
	
	%bullet.Position = %this.owner.getPosition();
	
	%bullet.setLinearVelocity( %shootDir );	
}
		
function ShipControls::shoot( %this, %val ) {
	if ( $pauseStatus ) {
		return;
	}

	if ( %val == 0 ) {
		return;
	}
	
	if ( LaserBar.canShoot ) {
		%this.bulletTimer();

		LaserBar.updateLaser( Ship.fireRate );
	}
}
