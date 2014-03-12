function createBackground() { 
	%scrolling = new SceneObjectSet( BackSet );
	%pos = "0 0";
	%scl = "200 150";
	
	%background = new Sprite( ParaBack ); 
	%background.setUpdateCallback( true );
	%background.setBodyType( dynamic );
	%background.Position = "0 0";
	%background.Size = "200 150";
	%background.SceneLayer = 31;
	%background.Image = "gameModule:farBack";
	
	%midground = new Sprite();
	%midground.setBodyType( dynamic );
	%midground.setBlendAlpha( 0.6 );
	%midground.Position = "0 0";
	%midground.Size = "200 150";
	%midground.SceneLayer = 30;
	%midground.Image = "gameModule:midBack";
	
	%foreground = new Sprite();
	%foreground.setName( "foreground" );
	%foreground.setBodyType( dynamic );
	%foreground.setBlendAlpha( 0.4 );
	%foreground.Position = "0 0";
	%foreground.Size = "200 150";
	%foreground.SceneLayer = 29;
	%foreground.Image = "gameModule:foreBack";
	
	%scrolling.add( %background, %midground, %foreground );
	
	GameScene.add( %background );
	GameScene.add( %midground );
	GameScene.add( %foreground );
}

function ParaBack::onUpdate( %this ) {
	%dir = Ship.getLinearVelocity();
	%xDir = getWord( %dir, 0 );
	%yDir = getWord( %dir, 1 );
	
	%mid = BackSet.getObject( 1 );
	%fore = BackSet.getObject( 2 );
	
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
}

