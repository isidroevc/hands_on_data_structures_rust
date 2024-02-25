

#[derive(Clone, Debug)]
pub struct IoTDevice {
    pub numerical_id: u64,
    pub path: String,
    pub address: String,
}

impl IoTDevice {
    pub fn new(id: u64, address: impl Into<String>, path: impl Into<String>) -> IoTDevice {
        IoTDevice {
            address: address.into(),
            numerical_id: id,
            path: path.into(),
        }
    }
}

impl PartialEq for IoTDevice {
    fn eq(&self, other: &IoTDevice) -> bool {
        self.numerical_id == other.numerical_id && self.address == other.address
    }
}

#[derive(Clone, Debug)]
pub struct MessageNotification {
    pub no_messages: u64,
    pub device: IoTDevice,
}

impl MessageNotification {
    pub fn new(device: IoTDevice, no_messages: u64) -> MessageNotification {
        MessageNotification {
            no_messages: no_messages,
            device: device,
        }
    }
}

impl PartialEq for MessageNotification {
    fn eq(&self, other: &MessageNotification) -> bool {
        self.device.eq(&other.device) && self.no_messages == other.no_messages
    }
}
