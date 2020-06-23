/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 * @flow
 */

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

import Analytics from 'appcenter-analytics';

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
      <Button title="asdf" onPress={toggle} />
    </View>
  );
  
  const actionRenderItem = ({ item: { title, action } }) => (
    <Button title={title} style={styles.sectionContainer} onPress={action} />
  );
  
  const [analyticsEnabled, setAnalyticsEnabled] = React.useState(false);

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
                      title: 'Analytics Enabled',
                      value: 'analyticsEnabled',
                      toggle: async () => {
                        //Alert.alert('Toggled \'Analytics Enabled\'');
                        //await Analytics.setEnabled(!this.state.analyticsEnabled);
                        //const analyticsEnabled = await Analytics.isEnabled();
                        
                        //const ae = await Analytics.isEnabled();
                        //Alert.alert('The retuned value is: ' + ae);
                        //setAnalyticsEnabled(!analyticsEnabled);

                        await Analytics.setEnabled(!analyticsEnabled);
                        //await Analytics.isEnabled();
                        setAnalyticsEnabled(await Analytics.isEnabled());
                        Alert.alert("Enabled: " + (await Analytics.isEnabled()));
                      }
                    },
                  ],
                  renderItem: switchRenderItem
                },
                {
                  title: 'Actions',
                  data: [
                    {
                      title: 'Track event without properties',
                      action: () => {
                        //Alert.alert('Pressed Track Event Without Properties');
                        const eventName = 'EventWithoutProperties';
                        Analytics.trackEvent(eventName);
                        //showEventToast(eventName);
                      }
                    },
                    {
                      title: 'Track event with properties',
                      action: () => {
                        //Alert.alert('Pressed Track Event With Properties');
                        const eventName = 'EventWithProperties';
                        Analytics.trackEvent(eventName, { property1: '100', property2: '200' });
                        //showEventToast(eventName);
                      }
                    },
                    {
                      title: 'Track event with long property value',
                      action: () => {
                        //Alert.alert('Pressed Track Event With Long Property Value');
                        const eventName = 'EventWithLongProperties';
                        Analytics.trackEvent(eventName, { propertyValueTooLong: '12345678901234567890123456789012345678901234567890123456789012345' });
                        //showEventToast(eventName);
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
