function createBackground() { 
	%scrolling = new SceneObjectSet( BackSet );
	%pos = "0 0";
	%scl = "200 150";
	
	%background = new Sprite( ParaBack ); 
	%background.name = "back";
	%background.setUpdateCallback( true );
	%background.setBodyType( dynamic );
	%background.Position = "0 0";
	%background.Size = "200 150";
	%background.SceneLayer = 31;
	%background.Image = "gameModule:farBack";
	
	%midground = new Sprite();
	%midground.name = "mid";
	%midground.setBodyType( dynamic );
	%midground.setBlendAlpha( 0.6 );
	%midground.Position = "0 0";
	%midground.Size = "200 150";
	%midground.SceneLayer = 30;
	%midground.Image = "gameModule:midBack";
	
	%foreground = new Sprite();
	%foreground.name = "fore";
	%foreground.setName( "foreground" );
	%foreground.setBodyType( dynamic );
	%foreground.setBlendAlpha( 0.4 );
	%foreground.Position = "0 0";
	%foreground.Size = "200 150";
	%foreground.SceneLayer = 29;
	%foreground.Image = "gameModule:foreBack";
	
	%scrolling.add( %background, %midground, %foreground );

	for ( %i = 0; %i != 24; %i++ ) {
		%back = new Sprite() {
			name = "back";
			size = "200 150";
			bodyType = dynamic;
			SceneLayer = 31;
		};

		%mid = new Sprite() {
			name = "mid";
			size = "200 150";
			bodyType = dynamic;
			SceneLayer = 30;
		};
		%mid.setBlendAlpha( 0.6 );

		%fore = new Sprite() { 
			name = "fore";
			size = "200 150";
			bodyType = dynamic;
			SceneLayer = 29;
		};
		%fore.setBlendAlpha( 0.4 );

		%back.copyFrom( %background );
		%mid.copyFrom( %midground );
		%fore.copyFrom( %foreground );

		%scrolling.add( %back, %mid, %fore );
	}

	setLimits();
	setBackPositions();
}

function setBackPositions() {
	%x = 100;
	%y = 75;

	for ( %i = 0; %i != BackSet.getCount(); %i++ ) {
		%tmp = BackSet.getObject( %i );
		if ( %i < 3 ) {
			%tmp.position = %x + getRandom( 0, 5 ) SPC %y + getRandom( 0, 5 );
		} else if ( %i >= 3 && %i < 6 ) {
			%tmp.position = ( %x * 2 ) + getRandom( 0, 5 ) SPC %y - getRandom( 0, 5 );
		} else if ( %i >= 6 && %i < 9 ) {
			%tmp.position = ( %x * 3 ) + getRandom( 0, 5 ) SPC %y + getRandom( 0, 5 );
		} else if ( %i >= 9 && %i < 12 ) {
			%tmp.position = %x + getRandom( 0, 5 ) SPC ( %y * 2 ) - getRandom( 0, 5 );
		} else if ( %i >= 12 && %i < 15 ) { 
			%tmp.position = ( %x * 2 ) + getRandom( 0, 5 ) SPC ( %y * 2 ) + getRandom( 0, 5 );
		} else if ( %i >= 15 && %i < 18 ) {
			%tmp.position = ( %x * 3 ) + getRandom( 0, 5 ) SPC ( %y * 2 ) - getRandom( 0, 5 );
		} else if ( %i >= 18 && %i < 21 ) {
			%tmp.position = %x - getRandom( 0, 5 ) SPC ( %y * 3 ) + getRandom( 0, 5 );
		} else if ( %i >= 21 && %i < 24 ) {
			%tmp.position = ( %x * 2 ) - getRandom( 0, 5 ) SPC ( %y * 3 ) - getRandom( 0, 5 );
		} else if ( %i >= 24 ) {
			%tmp.position = ( %x * 3 ) - getRandom( 0, 5 ) SPC ( %y * 3 ) + getRandom( 0, 5 );
		}
		GameScene.add( %tmp );
	}
}

function setLimits() {
	%left = new SceneObject();
	%left.name = "border";
	%left.createEdgeCollisionShape( 0, 0, 0, 75 );
	%left.position = 0 SPC $height * $scaleFactor;
	%left.setBodyType( static );
	GameScene.add( %left );
	
	%top = new SceneObject();
	%top.name = "border";
	%top.createEdgeCollisionShape( 0, 0, ( $width * $scaleFactor ) - 1, 0 );
	%top.position = 1 SPC ( $height * $scaleFactor ) + 75;
	%top.setBodyType( static );
	GameScene.add( %top );

	%right = new SceneObject();
	%right.name = "border";
	%right.createEdgeCollisionShape( 0, 0, 0, 75 );
	%right.position = $width * $scaleFactor SPC $height * $scaleFactor;
	%right.setBodyType( static );
	GameScene.add( %right );
}

function ParaBack::onUpdate( %this ) {
	%dir = Ship.getLinearVelocity();
	%xDir = getWord( %dir, 0 );
	%yDir = getWord( %dir, 1 );
	
	for ( %i = 0; %i != BackSet.getCount(); %i++ ) {
		%t = BackSet.getObject( %i );

		if ( %t.name $= "back" ) {
			continue;
		}

		if ( %t.name $= "mid" ) {
			if ( %xDir != 0 ) {
				%t.setLinearVelocityX( %xDir / 4 );
			}

			if ( %yDir != 0 ) {
				%t.setLinearVelocityY( %yDir / 4 );
			}
		}

		if ( %t.name $= "fore" ) {
			if ( %xDir != 0 ) {
				%t.setLinearVelocityX( %xDir / 2 );
			}

			if ( %yDir != 0 ) {
				%t.setLinearVelocityY( %yDir / 2 );
			}
		}
	}
}
/*
if ( %xDir != 0 ) {
	%mid.setLinearVelocityX( %xDir / 4.0 );
	%fore.setLinearVelocityX( %xDir / 2.0 );
} else { 
	%mid.setLinearVelocityX( 0.0 );
	%fore.setLinearVelocityX( 0.0 );
}

if ( %yDir != 0 ) {
	%mid.setLinearVelocityY( %yDir / 4.0 );
	%fore.setLinearVelocityY( %yDir/ 2.0 );
} else { 
	%mid.setLinearVelocityY( 0.0 );
	%fore.setLinearVelocityY( 0.0 );
}
*/
