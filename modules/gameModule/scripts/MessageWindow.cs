function messageWindowCreate( %message, %type ) {
	pause();
	
	%tmpWindow = new SceneWindow( MessageWindow ) {
		Profile = GuiDefaultProfile;
		size = "100 75";
		position = "0 0";
	};
	
	%tmpScene = new Scene( MessageScene );
	
	%background = new Sprite( MessageBox ) {
		class = "MessageBoxClass";
		image = "gameModule:messageBox";
		size = "75 15";
		position = "0 29";
		SceneLayer = "5";
		UpdateCallBack = true;
		Visible = false;
	};
	%tmpScene.add( %background );
	
	%tmpWindow.setScene( %tmpScene );
	Canvas.add( %tmpWindow );
	
	%scripter = new SceneObject( MessageScript ) {
		class = "MessageScriptClass";
		UpdateCallBack = true;
	};	
	%tmpScene.add( %scripter );
	
	%scripter.text = %message;
	%scripter.msgPos = 0;
	%scripter.deletePos = 50;
	%scripter.xOff = 0;
	%scripter.yOff = 0;
	%scripter.isTyping = true;
	%scripter.isMoving = true;
	
	%scripter.isFinishedTyping = false;

	MessageWindow.setupControls();

	switch( $type ) {
		case "item":
			echo( "working" );
		default:
			echo( "not found" );
	}
}

function MessageWindow::setupControls( %this ) { 
	%input = new ScriptObject( InputManager );
	%this.addInputListener( %input );

	%controls = new ActionMap( messageControls );
	%controls.bindCmd( keyboard, "enter", "nothing();", "MessageScript.continueText();" );
	%controls.push();
}

function MessageScriptClass::onUpdate( %this ) { 
	if ( %this.isMoving ) {
		%this.isMoving = false;
		
		MessageBox.setVisible( true );
		%this.startTimer( timerText, 50.0, strlen( %this.text ) );
	}
	
	if ( %this.deletePos <= 1 && %this.isTyping == false ) {
		%this.isTyping = true;
		%this.deletePos = 50;
		MessageScript.startTimer( timerText, 50.0, MessageScript.stringLeft );
	}
}

function MessageScriptClass::continueText( %this ) {
	if ( %this.isTyping == true && %this.isTimerActive() == true ){
		MessageScript.stopTimer();
		MessageScript.startTimer( timerText, 1.0, MessageScript.stringLeft );
		return;
	}
	
	if ( %this.isTyping == false && %this.isTimerActive() == false && %this.msgPos <= strlen( MessageScript.text ) ) {
		Flasher.stopTimer();
		Flasher.delete();
		
		%this.startTimer( timerTextDelete, 1.0, %this.msgPos );
		%this.deletePos = MessageScene.getCount() - 1;
	}
	
	if ( %this.isFinishedTyping == true ) {
		schedule( 10, 0, deleteMessageBox );
	}
}

function MessageScriptClass::timerText( %this ) { 
	if ( %this.msgPos >= strlen( %this.text ) - 1 ) {
		%this.stopTimer();
		%this.isFinishedTyping = true;
		%this.setUpdateCallBack( false );
		return;
	}
	
	//for the timer if someone clicks to speed up the text
	%this.stringLeft = strlen( %this.text ) - %this.msgPos;
	
	%j = strpos( %this.text, " ", %this.msgPos );
	%word = getSubStr( %this.text, %this.msgPos, %j - %this.msgPos + 1);
	
	if ( %this.xOff + strlen( %word ) > 36 ) {
		%this.xOff = 0;
		%this.yOff = %this.yOff + 3;
	}
	
	if ( %this.yOff == 12 ) {
		MessageScript.stopTimer();
		MessageScript.isTyping = false;
		%this.yOff = 0;
		
		%flasher = new Sprite( Flasher ) {
			class = "FlasherClass";
			image = "UIAssets:closeTile";
			size = "2 2";
			position = "36.5 23.5";
			BlendColor = "0.0 0.0 0.0 1.0";
		};
		MessageScene.add( %flasher );
		%flasher.startTimer( flash, 500 );
		return;
	}
	
	%x = -34 + ( %this.xOff * 2 );
	%this.xOff++;
	%y = 33.5 - %this.yOff;
	
	%letter = getSubStr( %this.text, %this.msgPos, 1 );
	
	%tmp = new ImageFont() {
		image = "gameModule:font";
		Text = %letter;
		FontSize = "2";
		Position = %x SPC %y;
		SceneLayer = "0";
		//BlendColor = "0, 0, 0";
		TextAlignment = "center";
	};
	MessageScene.add( %tmp );
	
	%this.msgPos++;
}

function MessageScriptClass::timerTextDelete( %this ) {
	%tmp = MessageScene.getObject( %this.deletePos );
	
	if ( %tmp.class $= "MessageBoxClass" || %tmp.class $= "MessageScriptClass" )
		return;
		
	%tmp.delete();
	%this.deletePos--;
}

function FlasherClass::flash( %this ) {
	if ( %this.getVisible() == true )
		%this.setVisible( false );
	else
		%this.setVisible( true );
}

function deleteMessageBox() {
	MessageScene.clear( true );
	messageControls.pop();
	InputManager.delete();
	MessageWindow.delete();

	unpause();
}

//no idea
function nothing() {
	return;
}
