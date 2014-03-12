function createMothership() {
	%mothership = new Sprite( Mothership );
	%mothership.name = "mothership";
	%mothership.Image = "gameModule:mothership";
	%mothership.SceneLayer = 0;
	%mothership.setBodyType( static );
	%mothership.createEdgeCollisionShape( -15, -12.5, 15, -12.5 );
	%mothership.createEdgeCollisionShape( -15, -12.5, -15, 4.5 );
	%mothership.createEdgeCollisionShape( 15, -12.5, 15, 4.5 );
	%mothership.createEdgeCollisionShape( -15, 4.5, -9, 12.5 );
	%mothership.createEdgeCollisionShape( 15, 4.5, 9, 12.5 );
	
	%mothership.setDefaultRestitution( 0.5 );
	%mothership.setSceneGroup( 1 );
	%mothership.setCollisionGroups( 1 );
	%mothership.setCollisionLayers( 1 );
	%mothership.Size = "30 25";
	
	return %mothership;
}

function Mothership::createDropBox( %this ) {
	%dropBox = new SceneObject( DropBox );
	%dropBox.name = "dropBox";
	%dropBox.setBodyType( static );
	%dropBox.size = "25 10";
	%dropBox.createPolygonBoxCollisionShape();
	%dropbox.setPositionX( getWord( %this.getPosition(), 0 ) );
	%dropbox.setPositionY( getWord( %this.getPosition(), 1 ) - 5 );
	%dropBox.setSceneGroup( 2 );
	%dropBox.setCollisionCallback( true );
	
	GameScene.add( %dropBox );
}

function DropBox::onCollision( %this, %collides, %details ) {
	if ( %collides.name $= "fuelItem" ) {
		%collides.safeDelete();
		FuelBar.updateFuel();
	}
}
