function createSceneWindow() {
		if ( !isObject( Window ) ) {
				new SceneWindow( Window );
				
				Window.Profile = GuiDefaultProfile;
				
				Canvas.setContent( Window );
		}
		
		Window.setCameraPosition( 0, 0 );
		Window.setCameraSize( 100, 75 );
		Window.setCameraZoom( 1 );
		Window.setCameraAngle( 0 );
}

function destroySceneWindow() {
		if ( !isObject( Window ) ) {
				return;
		}
		
		Window.delete();
}

