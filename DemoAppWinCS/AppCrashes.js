// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import React from 'react';
import {
  SafeAreaView,
  StyleSheet,
  ScrollView,
  View,
  Text,
  SectionList,
  StatusBar,
  Switch,
  Button,
  TouchableOpacity,
  Alert,
} from 'react-native';

import Crashes, { UserConfirmation, ErrorAttachmentLog } from 'appcenter-crashes';

//import AttachmentsProvider from '../AttachmentsProvider';
//mport SharedStyles from '../SharedStyles';
//import CrashesTabBarIcon from '../assets/crashes.png';

import {
  Header,
  LearnMoreLinks,
  Colors,
  DebugInstructions,
  ReloadInstructions,
} from 'react-native/Libraries/NewAppScreen';

const App: () => React$Node = () => {
  const switchRenderItem = ({ item: { title, value, toggle } }) => (
    <View style={styles.sectionContainer.item}>
      <Text style={styles.sectionDescription}>{title}</Text>
      <Button title="Enable/Disable Crashes" onPress={toggle} />
    </View>
  );
  
  const actionRenderItem = ({ item: { title, action } }) => (
    <Button title={title} style={styles.sectionContainer} onPress={action} />
  );
  
  const [crashesEnabled, setCrashesEnabled] = React.useState(false);

  Crashes.setListener({
      onBeforeSending: function (report) {
        Alert.alert('onBeforeSending');
      },
      onSendingSucceeded: function (report) {
        Alert.alert('onSendingSucceeded');
      },
      onSendingFailed: function (report) {
        Alert.alert('onSendingFailed');
      },
      getErrorAttachments(report) {
        const textAttachment = ErrorAttachmentLog.attachmentWithText('Hello text attachment!', 'hello.txt');
        const binaryAttachment = ErrorAttachmentLog.attachmentWithBinary(`${imageAsBase64string}`, 'logo.png', 'image/png');
        return [textAttachment, binaryAttachment];
      },
      shouldAwaitUserConfirmation: function (report) {
        return false;
    },
  });

  return (
    <>
      <StatusBar barStyle="dark-content" />
      <SafeAreaView>
        <ScrollView
          contentInsetAdjustmentBehavior="automatic"
          style={styles.scrollView}>
          {global.HermesInternal == null ? null : (
            <View style={styles.engine}>
              <Text style={styles.footer}>Engine: Hermes</Text>
            </View>
          )}
          <View style={styles.body}>
            <SectionList
              renderItem={({ item }) => <Text style={[styles.sectionContainer, styles.sectionHeader]}>{item}</Text>}
              renderSectionHeader={({ section: { title } }) => <Text style={styles.sectionHeader}>{title}</Text>}
              keyExtractor={(item, index) => item + index}
              sections={[
                {
                  title: 'Settings',
                  data: [
                    {
                      title: 'Crashes Enabled',
                      value: 'crashesEnabled',
                      toggle: async () => {
                        await Crashes.setEnabled(!crashesEnabled);
                        setCrashesEnabled(await Crashes.isEnabled());
                        Alert.alert("Enabled: " + (await Crashes.isEnabled()));
                      }
                    },
                  ],
                  renderItem: switchRenderItem
                },
                {
                  title: 'Actions',
                  data: [
                    {
                      title: 'Has Crashed?',
                      action: async () => {
                        const result = await Crashes.hasCrashedInLastSession()
                        if (result) {
                          const crashReport = await Crashes.lastSessionCrashReport();
                          const crashReportString = JSON.stringify(crashReport, null, 4);
                          Alert.alert('App crashed in the last session. Crashes.lastSessionCrashReport(): ', crashReportString);
                        } else {
                          Alert.alert("Has Crashed: " + result);
                        }
                      }
                    },
                    {
                      title: 'Past Memory Warning?',
                      action: async () => {
                        const result = await Crashes.hasReceivedMemoryWarningInLastSession();
                        Alert.alert("Has recieved memory warning in last session: " + result);
                      }
                    },
                    {
                      title: 'Generate Test Crash',
                      action: async () => {
                        await Crashes.generateTestCrash();
                      }
                    },
                    {
                      title: 'Don\'t Send Crash Reports',
                      action: () => {
                        Crashes.notifyUserConfirmation(UserConfirmation.DONT_SEND);
                      }
                    },
                    {
                      title: 'Send Crash Reports',
                      action: () => {
                        Crashes.notifyUserConfirmation(UserConfirmation.SEND);
                      }
                    },
                    {
                      title: 'Always Send Crash Reports',
                      action: () => {
                        Crashes.notifyUserConfirmation(UserConfirmation.ALWAYS_SEND);
                      }
                    },
                  ],
                  renderItem: actionRenderItem
                },
              ]}
            />
          </View>
        </ScrollView>
      </SafeAreaView>
    </>
  );
};

const styles = StyleSheet.create({
  scrollView: {
    backgroundColor: Colors.lighter,
  },
  engine: {
    position: 'absolute',
    right: 0,
  },
  body: {
    backgroundColor: Colors.white,
  },
  sectionContainer: {
    marginTop: 32,
    paddingHorizontal: 24,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: '600',
    color: Colors.black,
  },
  sectionDescription: {
    marginTop: 8,
    fontSize: 18,
    fontWeight: '400',
    color: Colors.dark,
  },
  highlight: {
    fontWeight: '700',
  },
  footer: {
    color: Colors.dark,
    fontSize: 12,
    fontWeight: '600',
    padding: 4,
    paddingRight: 12,
    textAlign: 'right',
  },
});

export default App;


