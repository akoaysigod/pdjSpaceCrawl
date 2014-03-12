function createItem() { 
	%item = new Sprite( Item );
	%item.name = "fuelItem";
	%item.Image = "gameModule:fuel";
	%item.SceneLayer = 5;
	%item.Size = "4 7";
	%item.setBodyType( dynamic );
	%item.createPolygonBoxCollisionShape();
	%item.setCollisionLayers( 1, 2, 10 );
	%item.setCollisionGroups( 1, 2, 10 );
	%item.setSceneGroup( 1 );	
	%item.setUpdateCallback( true );
	%item.setFixedAngle( true );
	
	%item.joint = -1;
	return %item;
}

function Item::findAngle( %this, %angleTo ) {
	%angle = Vector2AngleToPoint( %angleTo, %this.getPosition() );
	%this.line.setAngle( %angle );
}

function Item::onUpdate( %this ) { 
	%dist = VectorDist( %this.getPosition(), Ship.getPosition() ); 

	if ( GameScene.getJointCount() ) {
		%this.findAngle( Ship.getPosition() );
		%this.line.position = %this.getPosition();
	}

	if ( %dist < 11 && %this.joint == -1 ) {
		%this.joint = GameScene.createDistanceJoint( Ship, %this, "0 0", "0 0", 10, 0.0, 1.0, false );

		%this.line = new ShapeVector();
		%this.line.PolyList = "0 10 0.1 10 0.1 0 0 0";
		%this.line.position = %this.getPosition();
		%this.line.SceneLayer = 6;
		%this.findAngle( Ship.getPosition() );
		%this.line.position = %this.getPosition();
		GameScene.add( %this.line );
	}
	
	if ( getWord( %this.getPosition(), 1 ) > getWord( Mothership.getPosition(), 1 ) + 15 ) {
		if ( mAbs( getWord( %this.getPosition(), 0 ) - getWord( Mothership.getPosition(), 0 ) ) < 5 ) {
			if ( %this.joint != -1 ) {
				%wtf = GameScene.deleteJoint( %this.joint );
				%this.setLinearVelocity( "0 -5" );
				%this.setUpdateCallback( false );
				%this.line.safeDelete();
			}
		}
	}
}