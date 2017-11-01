using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Blink : MonoBehaviour {
	public Text mytext; 
	Color BLUE = new Color(0,0,1.0f,1.0f);
	Color BLACK = new Color(0,0,0,1.0f);
	
	/* Connected Sphero Robot */
	Sphero[] m_Spheros;

	
	/* Counter to determine if Sphero should have color or not */
	int m_BlinkCounter;
	
	/* Use this for initialization */
	void ViewSetup() {
		// Get Connected Sphero

	}
	
	void Start () {	
		mytext.text= "hello0 ";
	
		try{
		m_Spheros = SpheroProvider.GetSharedProvider().GetConnectedSpheros();
		}
		catch ( Exception e) {
			mytext.text = e.ToString();
		}
			//mytext.text= "hellocc ";
		SpheroDeviceMessenger.SharedInstance.NotificationReceived += ReceiveNotificationMessage;
		//mytext.text= "hello3 ";
		if (m_Spheros.Length == 0)
		{	//mytext.text= "hello2 ";
			SceneManager.LoadScene("SpheroConnectionScene");//Application.LoadLevel("SpheroConnectionScene");
			//mytext.text= "hello ";
		}
	}
	
	/* This is called when the application returns from or enters background */
	void OnApplicationPause(bool pause) {
		//mytext.text= "paused "+pause;
		if( pause ) {
			SpheroProvider.GetSharedProvider().DisconnectSpheros();
			// Initialize the device messenger which sets up the callback
			SpheroDeviceMessenger.SharedInstance.NotificationReceived -= ReceiveNotificationMessage;
		}
		else {
			mytext.text= "paused ";
		}
	}
	
	/* Update is called once per frame */
	void Update () {
		m_BlinkCounter++;
		if( m_BlinkCounter % 20 == 0 ) {			
			foreach( Sphero sphero in m_Spheros ) {
				// Set the Sphero color to blue 
				if( sphero.RGBLEDColor.Equals(BLACK) ) {
					sphero.SetRGBLED(BLUE.r,BLUE.g,BLUE.b);
				}
				else {
					sphero.SetRGBLED(BLACK.r,BLACK.g,BLACK.b);	
				}
			}
		}
	}

	/*
	 * Callback to receive connection notifications 
	 */
	private void ReceiveNotificationMessage(object sender, SpheroDeviceMessenger.MessengerEventArgs eventArgs)
	{
		SpheroDeviceNotification message = (SpheroDeviceNotification)eventArgs.Message;
		Sphero notifiedSphero = SpheroProvider.GetSharedProvider().GetSphero(message.RobotID);
		if( message.NotificationType == SpheroDeviceNotification.SpheroNotificationType.DISCONNECTED ) {
			notifiedSphero.ConnectionState = Sphero.Connection_State.Disconnected;
			Application.LoadLevel("NoSpheroConnectedScene");
		}
	}
}